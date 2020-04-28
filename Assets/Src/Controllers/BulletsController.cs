using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class BulletsController : IInitializable, ITickable, IDisposable
{
    private EventsAggregator _eventsAggregator;
    private ScreenBoundsProvider _screenBoundsProvider;
    private IBulletsModelProvider _bulletsModelProvider;
    private IEnemyShipModelsProvider _enemyShipModelsProvider;
    private IPlayerShipModelProvider _playerShipModelsProvider;
    private IViewManager _viewManager;

    [Inject]
    public void Construct(
        EventsAggregator eventsAggregator,
        ScreenBoundsProvider screenBoundsProvider,
        IBulletsModelProvider bulletsModelProvider,
        IEnemyShipModelsProvider enemyShipModelsProvider,
        IPlayerShipModelProvider playerShipModelsProvider,
        IViewManager viewManager)
    {
        _eventsAggregator = eventsAggregator;
        _screenBoundsProvider = screenBoundsProvider;
        _bulletsModelProvider = bulletsModelProvider;
        _enemyShipModelsProvider = enemyShipModelsProvider;
        _playerShipModelsProvider = playerShipModelsProvider;
        _viewManager = viewManager;
    }

    public void Initialize()
    {
        _eventsAggregator.BulletViewCreated += OnBulletViewCreated;
    }

    public void Dispose()
    {
        _eventsAggregator.BulletViewCreated -= OnBulletViewCreated;
    }

    public void Tick()
    {
        var i = 0;
        while (i < _bulletsModelProvider.Models.Count)
        {
            var bulletModel = _bulletsModelProvider.Models[i];
            if (ProcessBounds(bulletModel) || ProcessCollisions(bulletModel))
            {
                RemoveBullet(i, bulletModel.Transform.gameObject);
                continue;
            }
            bulletModel.Transform.position += bulletModel.Transform.forward.normalized * bulletModel.Speed;
            i++;
        }
    }

    private bool ProcessBounds(BulletModel bulletModel)
    {
        var bulletPosition = bulletModel.Transform.position;
        return (new Vector2(bulletPosition.x, bulletPosition.z) - _screenBoundsProvider.Bounds.center).magnitude > _screenBoundsProvider.Bounds.height * 2;
    }

    private void RemoveBullet(int modelIndex, GameObject gameObject)
    {
        _bulletsModelProvider.Models.RemoveAt(modelIndex);
        _viewManager.Destroy(gameObject);
    }

    private bool ProcessCollisions(BulletModel bulletModel)
    {
        var playerShipModel = _playerShipModelsProvider.ShipModel;
        if (bulletModel.Team == playerShipModel.Team)
        {
            if (_enemyShipModelsProvider.Models.Any(m => m.HP > 0))
            {
                var bulletPosition = bulletModel.Transform.position;
                var closestEnemy = _enemyShipModelsProvider.Models
                    .Where(m => m.HP > 0)
                    .DefaultIfEmpty()
                    .Aggregate((min, m) =>
                        min == null
                        || (m.Position - bulletPosition).magnitude < (min.Position - bulletPosition).magnitude
                            ? m
                            : min);
                if (HitTestPoint(closestEnemy, bulletPosition))
                {
                    _viewManager.Instantiate(bulletModel.SparksPrefab, bulletPosition, bulletModel.Transform.rotation);
                    closestEnemy.HP -= bulletModel.Damage;
                    return true;
                }
            }
        }
        else
        {
            if (HitTestPoint(playerShipModel, bulletModel.Transform.position))
            {
                _viewManager.Instantiate(bulletModel.SparksPrefab, bulletModel.Transform.position, bulletModel.Transform.rotation);
                playerShipModel.HP -= bulletModel.Damage;
                return true;
            }
        }

        return false;
    }

    private bool HitTestPoint(ShipModel shipModel, Vector3 position)
    {
        return shipModel.Colliders.Any(c => c.bounds.Contains(position));
    }

    private void OnBulletViewCreated(Transform bulletTransform, WeaponConfig weaponConfig, TeamId team)
    {
        var bulletModel = new BulletModel(bulletTransform, weaponConfig, team);
        _bulletsModelProvider.Add(bulletModel);
    }
}
