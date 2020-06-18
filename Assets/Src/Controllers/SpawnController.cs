using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;

public class SpawnController : MonoBehaviour, IInitializable
{    private IUnitFactory _unitFactory;
    private PlayerDataModel _playerDataModel;
    private LevelConfig _currentLevelConfig;

    [Inject]
    public void Construct(
        IUnitFactory unitFactory,
        PlayerDataModel playerDataModel,
        CurrentLevelConfigHolder currentLevelConfigHolder)
    {
        _unitFactory = unitFactory;
        _playerDataModel = playerDataModel;
        _currentLevelConfig = currentLevelConfigHolder.CurrentLevelConfig;
    }

    public void Initialize()
    {
        _unitFactory.CreatePlayerShip(_playerDataModel.ShipData.ToShipDataMin());
        StartCoroutine(SpawEnemies());
    }

    private IEnumerator SpawEnemies()
    {
        yield return new WaitForSeconds(_currentLevelConfig.TimeBetweenWaves);

        foreach (var wave in _currentLevelConfig.Waves)
        {
            var shipsRestAmouts = wave.ShipGroups.Select(g => g.Amount).ToArray();
            while (shipsRestAmouts.Any(a => a > 0))
            {
                var index = 0;
                do
                {
                    index = (int)(Random.value * shipsRestAmouts.Length);
                }
                while (shipsRestAmouts[index] <= 0);

                var shipGroup = wave.ShipGroups[index];
                _unitFactory.CreateEnemyShip(shipGroup.Ship.ShipDataMin);
                shipsRestAmouts[index]--;

                yield return new WaitForSeconds(wave.TimeBetweenSpawn);
            }

            yield return new WaitForSeconds(_currentLevelConfig.TimeBetweenWaves);
        }
    }
}
