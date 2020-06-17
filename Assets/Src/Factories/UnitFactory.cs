using System;
using UnityEngine;
using Zenject;

public interface IUnitFactory
{
    PlayerShipModel CreatePlayerShip(ShipDataMin shipData);
    EnemyShipModel CreateEnemyShip(ShipDataMin shipData);
}

public class UnitFactory : MonoBehaviour, IUnitFactory
{
    private ModelsFactory _modelsFactory;
    private PlayerShipModelHolder _playerShipModelHolder;
    private EnemyShipModelsProvider _enemyShipsModelsProvider;
    private ScreenBoundsProvider _screenBoundsProvider;
    private DiContainer _diContainer;

    [Inject]
    public void Construct(
        ModelsFactory modelsFactory,
        PlayerShipModelHolder playerShipModelHolder,
        EnemyShipModelsProvider enemyShipsModelsProvider,
        ScreenBoundsProvider screenBoundsProvider,
        DiContainer diContainer)
    {
        //TODO: use Constructor

        _modelsFactory = modelsFactory;
        _playerShipModelHolder = playerShipModelHolder;
        _enemyShipsModelsProvider = enemyShipsModelsProvider;
        _screenBoundsProvider = screenBoundsProvider;
        _diContainer = diContainer;
    }

    public PlayerShipModel CreatePlayerShip(ShipDataMin shipDataMin)
    {
        var shipData = _modelsFactory.CreateShipData(shipDataMin);
        return CreatePlayerShip(shipData);
    }

    public PlayerShipModel CreatePlayerShip(ShipData shipData)
    {
        var model = new PlayerShipModel(shipData);
        _playerShipModelHolder.ShipModel = model;
        _diContainer.Instantiate<ShipMediator>(new object[] { model });

        return model;
    }

    public EnemyShipModel CreateEnemyShip(ShipDataMin shipDataMin)
    {
        var shipData = _modelsFactory.CreateShipData(shipDataMin);

        var model = new EnemyShipModel(shipData);
        _enemyShipsModelsProvider.AddShip(model);
        _diContainer.Instantiate<ShipMediator>(new object[] { model });

        model.Rotation = Quaternion.LookRotation(-model.Forward, Vector3.up);
        var rndPos = UnityEngine.Random.insideUnitCircle * _screenBoundsProvider.Bounds.width;
        rndPos.y += _screenBoundsProvider.Bounds.center.y;
        model.Position = new Vector3(rndPos.x, 0, rndPos.y + Math.Max(_screenBoundsProvider.Bounds.height, _screenBoundsProvider.Bounds.width));

        return model;
    }
}
