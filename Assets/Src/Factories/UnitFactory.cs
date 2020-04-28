using System;
using System.Linq;
using UnityEngine;
using Zenject;

public interface IUnitFactory
{
    PlayerShipModel CreatePlayerShip(ShipDataMin shipData);
    EnemyShipModel CreateEnemyShip(ShipDataMin shipData);
}

public class UnitFactory : MonoBehaviour, IUnitFactory
{
    private ShipsConfigProvider _shipsConfigProvider;
    private WeaponsConfigProvider _weaponsConfigProvider;
    private PlayerShipModelHolder _playerShipModelHolder;
    private EnemyShipModelsProvider _enemyShipsModelsProvider;
    private ScreenBoundsProvider _screenBoundsProvider;
    private DiContainer _diContainer;

    [Inject]
    public void Construct(
        ShipsConfigProvider shipsConfigProvider,
        WeaponsConfigProvider weaponsConfigProvider,
        PlayerShipModelHolder playerShipModelHolder,
        EnemyShipModelsProvider enemyShipsModelsProvider,
        ScreenBoundsProvider screenBoundsProvider,
        DiContainer diContainer)
    {
        _shipsConfigProvider = shipsConfigProvider;
        _playerShipModelHolder = playerShipModelHolder;
        _enemyShipsModelsProvider = enemyShipsModelsProvider;
        _screenBoundsProvider = screenBoundsProvider;
        _weaponsConfigProvider = weaponsConfigProvider;
        _diContainer = diContainer;
    }

    public PlayerShipModel CreatePlayerShip(ShipDataMin shipDataMin)
    {
        var (go, colliders, staticData) = CreateShip(shipDataMin);
        var model = new PlayerShipModel(colliders, staticData);
        _playerShipModelHolder.ShipModel = model;
        _diContainer.Instantiate<ShipMediator>(new object[] { model, go });

        return model;
    }

    public EnemyShipModel CreateEnemyShip(ShipDataMin shipDataMin)
    {
        var (go, colliders, shipData) = CreateShip(shipDataMin);
        var model = new EnemyShipModel(colliders, shipData);
        _enemyShipsModelsProvider.AddShip(model);
        _diContainer.Instantiate<ShipMediator>(new object[] { model, go });

        model.Rotation = Quaternion.LookRotation(-model.Forward, Vector3.up);
        var rndPos = UnityEngine.Random.insideUnitCircle * _screenBoundsProvider.Bounds.width;
        rndPos.y += _screenBoundsProvider.Bounds.center.y;
        model.Position = new Vector3(rndPos.x, 0, rndPos.y + Math.Max(_screenBoundsProvider.Bounds.height, _screenBoundsProvider.Bounds.width));

        return model;
    }

    private (GameObject, Collider[], ShipData) CreateShip(ShipDataMin shipDataMin)
    {
        var shipConfig = _shipsConfigProvider.GetConfigByShipType(shipDataMin.ShipType);
        var weaponsConfig = shipDataMin.WeaponIds.Select(_weaponsConfigProvider.GetConfigByWeaponId).ToArray();
        var shipData = new ShipData(shipConfig, weaponsConfig);

        var go = Instantiate(shipConfig.ShipPrefab);
        var colliders = go.GetComponent<CollidersComponent>().Colliders;

        return (go, colliders, shipData);
    }
}
