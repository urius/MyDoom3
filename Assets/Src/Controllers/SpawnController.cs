using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class SpawnController : MonoBehaviour, IInitializable
{
    [SerializeField]
    private ShipDataMin _playerShipDataTemp; //Temp: remove when created player data holder
    [SerializeField]
    private ShipDataMin _enemyShipDataTemp; //Temp: remove when created enemy levels congig

    private IUnitFactory _unitFactory;

    [Inject]
    public void Construct(IUnitFactory unitFactory)
    {
        _unitFactory = unitFactory;
    }

    public void Initialize()
    {
        _unitFactory.CreatePlayerShip(_playerShipDataTemp);
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
