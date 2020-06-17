using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class SpawnController : MonoBehaviour, IInitializable
{
    [SerializeField]
    private ShipDataMin _enemyShipDataTemp; //Temp: remove when created enemy levels congig

    private IUnitFactory _unitFactory;
    private PlayerDataModel _playerDataModel;

    [Inject]
    public void Construct(
        IUnitFactory unitFactory,
        PlayerDataModel playerDataModel)
    {
        _unitFactory = unitFactory;
        _playerDataModel = playerDataModel;
    }

    public void Initialize()
    {
        _unitFactory.CreatePlayerShip(_playerDataModel.ShipData.ToShipDataMin());
        StartCoroutine(SpawEnemy());
    }

    private IEnumerator SpawEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            _unitFactory.CreateEnemyShip(_enemyShipDataTemp);
        }
    }
}
