using System;
using UnityEngine;
using Zenject;

public class BulletsController : IInitializable, ITickable, IDisposable
{
    private EventsAggregator _eventsAggregator;
    private ScreenBoundsProvider _screenBoundsProvider;
    private IBulletsModelProvider _bulletsModelProvider;
    private IViewManager _viewManager;

    [Inject]
    public void Construct(
        EventsAggregator eventsAggregator,
        ScreenBoundsProvider screenBoundsProvider,
        IBulletsModelProvider bulletsModelProvider,
        IViewManager viewManager)
    {
        _eventsAggregator = eventsAggregator;
        _screenBoundsProvider = screenBoundsProvider;
        _bulletsModelProvider = bulletsModelProvider;
        _viewManager = viewManager;
    }

    public void Initialize()
    {
        _eventsAggregator.BulletViewCreated += OnBulletViewCreated;
        _eventsAggregator.CollisionHappened += OnCollision;
    }

    public void Dispose()
    {
        _eventsAggregator.BulletViewCreated -= OnBulletViewCreated;
        _eventsAggregator.CollisionHappened -= OnCollision;
    }

    public void Tick()
    {
        var i = 0;
        while (i < _bulletsModelProvider.Models.Count)
        {
            var bulletModel = _bulletsModelProvider.Models[i];
            if (CheckOutOfBounds(bulletModel))
            {
                RemoveBullet(i, bulletModel.Transform.gameObject);
                continue;
            }

            bulletModel.Transform.position += bulletModel.Transform.forward.normalized * bulletModel.Speed;
            i++;
        }
    }

    private bool CheckOutOfBounds(BulletModel bulletModel)
    {
        var bulletPosition = bulletModel.Transform.position;
        return (new Vector2(bulletPosition.x, bulletPosition.z) - _screenBoundsProvider.Bounds.center).magnitude > _screenBoundsProvider.Bounds.height * 2;
    }

    private void RemoveBullet(int modelIndex, GameObject gameObject)
    {
        _bulletsModelProvider.Models.RemoveAt(modelIndex);
        _viewManager.Destroy(gameObject);
    }

    private void OnCollision(ShipModel shipModel, BulletModel bulletModel)
    {
        _viewManager.Instantiate(bulletModel.SparksPrefab, bulletModel.Transform.position, bulletModel.Transform.rotation);
        shipModel.HP -= bulletModel.Damage;
        _bulletsModelProvider.Models.Remove(bulletModel);
        _viewManager.Destroy(bulletModel.Transform.gameObject);
    }

    private void OnBulletViewCreated(Transform bulletTransform, WeaponConfig weaponConfig, TeamId team)
    {
        var bulletModel = new BulletModel(bulletTransform, weaponConfig, team);
        _bulletsModelProvider.Add(bulletModel);
    }
}
