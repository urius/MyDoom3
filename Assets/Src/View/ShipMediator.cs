using System.Linq;
using UnityEngine;

public class ShipMediator
{
    private readonly ShipModel _shipModel;
    private readonly Transform[] _weapons;
    private readonly Collider[] _colliders;
    private readonly IViewManager _viewManager;
    private readonly IBulletsModelProvider _bulletsModelProvider;
    private readonly EventsAggregator _eventsAggregator;

    private GameObject _currentView;

    public ShipMediator(
        ShipModel shipModel,
        EventsAggregator eventsAggregator,
        IViewManager viewManager,
        IBulletsModelProvider bulletsModelProvider)
    {
        _shipModel = shipModel;

        _eventsAggregator = eventsAggregator;
        _viewManager = viewManager;
        _bulletsModelProvider = bulletsModelProvider;

        _currentView = GameObject.Instantiate(shipModel.ShipPrefab);
        _weapons = _currentView.GetComponent<WeaponsComponent>().WeaponTransforms;
        _colliders = _currentView.GetComponent<CollidersComponent>().Colliders;

        Activate();
    }

    private void OnModelDisposed()
    {
        Deactivate();

        GameObject.Destroy(_currentView);
    }

    private void Activate()
    {
        _eventsAggregator.Ticked += OnTicked;
        _shipModel.Disposed += OnModelDisposed;
        _shipModel.PositionChanged += OnPositionChanged;
        _shipModel.RotationChanged += OnRotationChanged;
        _shipModel.Fired += OnFired;
        _shipModel.DestroyStarted += OnDestroyShip;
    }

    private void Deactivate()
    {
        _eventsAggregator.Ticked -= OnTicked;
        _shipModel.Disposed -= OnModelDisposed;
        _shipModel.PositionChanged -= OnPositionChanged;
        _shipModel.RotationChanged -= OnRotationChanged;
        _shipModel.Fired -= OnFired;
        _shipModel.DestroyStarted -= OnDestroyShip;
    }

    private void OnTicked()
    {
        if (!_shipModel.IsDestroyState && _bulletsModelProvider.Models.Any(b => b.Team != _shipModel.Team))
        {
            var shipPosition = _shipModel.Position;
            var closestBullet = _bulletsModelProvider.Models
                        .Where(b => b.Team != _shipModel.Team)
                        .Aggregate((min, m) =>
                            min == null
                            || (m.Transform.position - shipPosition).magnitude < (min.Transform.position - shipPosition).magnitude
                                ? m
                                : min);
            if (_colliders.Any(c => c.bounds.Contains(closestBullet.Transform.position)))
            {
                _eventsAggregator.CollisionHappened(_shipModel, closestBullet);
            }
        }
    }

    private async void OnDestroyShip()
    {
        _viewManager.Instantiate(_shipModel.ExplosionPrefab, _currentView.transform.position, _currentView.transform.rotation);
        var view = _currentView;
        _currentView = GameObject.Instantiate(_shipModel.DestroyedShipPrefab, _shipModel.Position, _shipModel.Rotation);
        GameObject.Destroy(view);
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
