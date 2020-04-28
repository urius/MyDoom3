using System;
using UnityEngine;
using Zenject;

public class ShipMediator
{
    private readonly ShipModel _shipModel;
    private readonly GameObject _notDestroyedView;
    private readonly Transform[] _weapons;

    private EventsAggregator _eventsAggregator;
    private GameObject _currentView;
    private IViewManager _viewManager;

    public ShipMediator(ShipModel shipModel, GameObject notDestroyedView)
    {
        _shipModel = shipModel;
        _notDestroyedView = notDestroyedView;
        _currentView = _notDestroyedView;

        _weapons = _notDestroyedView.GetComponent<WeaponsComponent>().WeaponTransforms;
    }

    [Inject]
    public void Construct(EventsAggregator eventsAggregator, IViewManager viewManager)
    {
        _eventsAggregator = eventsAggregator;
        _viewManager = viewManager;

        Activate();
    }

    private void OnModelDisposed()
    {
        Deactivate();

        GameObject.Destroy(_currentView);
    }

    private void Activate()
    {
        _shipModel.Disposed += OnModelDisposed;
        _shipModel.PositionChanged += OnPositionChanged;
        _shipModel.RotationChanged += OnRotationChanged;
        _shipModel.Fired += OnFired;
        _shipModel.DestroyStarted += OnDestroyShip;
    }

    private void Deactivate()
    {
        _shipModel.Disposed -= OnModelDisposed;
        _shipModel.PositionChanged -= OnPositionChanged;
        _shipModel.RotationChanged -= OnRotationChanged;
        _shipModel.Fired -= OnFired;
        _shipModel.DestroyStarted -= OnDestroyShip;
    }

    private void OnDestroyShip()
    {
        _viewManager.Instantiate(_shipModel.ExplosionPrefab, _currentView.transform.position, _currentView.transform.rotation);
    }

    private void OnFired(int weaponIndex, WeaponConfig weaponConfig, TeamId team)
    {
        var weaponTransform = _weapons[weaponIndex];
        var bullet = _viewManager.Instantiate(weaponConfig.BulletPrefab, weaponTransform.position, weaponTransform.rotation);
        _eventsAggregator.BulletViewCreated(bullet.transform, weaponConfig, team);
    }

    private void OnPositionChanged()
    {
        _currentView.transform.position = _shipModel.Position;
    }

    private void OnRotationChanged()
    {
        _currentView.transform.rotation = _shipModel.Rotation;
    }
}
