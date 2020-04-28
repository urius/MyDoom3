using UnityEngine;
using Zenject;

public class ShipMediator
{
    private readonly ShipModel _shipModel;
    private readonly GameObject _initialView;
    private readonly Transform[] _weapons;

    private EventsAggregator _eventsAggregator;
    private IViewManager _viewManager;

    public ShipMediator(ShipModel shipModel, GameObject initialView)
    {
        _shipModel = shipModel;
        _initialView = initialView;

        _weapons = _initialView.GetComponent<WeaponsComponent>().WeaponTransforms;
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

        GameObject.Destroy(_initialView);
    }

    private void Activate()
    {
        _shipModel.Disposed += OnModelDisposed;
        _shipModel.PositionChanged += OnPositionChanged;
        _shipModel.RotationChanged += OnRotationChanged;
        _shipModel.Fired += OnFired;
    }

    private void Deactivate()
    {
        _shipModel.Disposed -= OnModelDisposed;
        _shipModel.PositionChanged -= OnPositionChanged;
        _shipModel.RotationChanged -= OnRotationChanged;
        _shipModel.Fired -= OnFired;
    }

    private void OnFired(int weaponIndex, WeaponConfig weaponConfig, TeamId team)
    {
        var weaponTransform = _weapons[weaponIndex];
        var bullet = _viewManager.Instantiate(weaponConfig.BulletPrefab, weaponTransform.position, weaponTransform.rotation);
        _eventsAggregator.BulletViewCreated(bullet.transform, weaponConfig, team);
    }

    private void OnPositionChanged()
    {
        _initialView.transform.position = _shipModel.Position;
    }

    private void OnRotationChanged()
    {
        _initialView.transform.rotation = _shipModel.Rotation;
    }
}
