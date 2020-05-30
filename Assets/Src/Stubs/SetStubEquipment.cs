using UnityEngine;
using Zenject;

public class SetStubEquipment : MonoBehaviour
{
    private PlayerDataModel _playerDataModel;
    private ShieldsConfigProvider _shieldsConfigProvider;
    private ShipsConfigProvider _shipsConfigProvider;

    [Inject]
    public void Construct(
        PlayerDataModel playerDataModel,
        ShieldsConfigProvider shieldsConfigProvider,
        ShipsConfigProvider shipsConfigProvider)
    {
        _playerDataModel = playerDataModel;
        _shieldsConfigProvider = shieldsConfigProvider;
        _shipsConfigProvider = shipsConfigProvider;

        InitStub();
    }

    void InitStub()
    {
        var shipConfig = _shipsConfigProvider.GetConfigByShipType(ShipType.StarSparrow);
        var numWeapons = shipConfig.ShipPrefab.GetComponent<WeaponsComponent>().WeaponTransforms.Length;
        Debug.Log("weapons count: " + numWeapons);
        _playerDataModel.ShipData = new ShipData(shipConfig, new WeaponConfig[numWeapons]);
        
        _playerDataModel.AddInventoryEquipment(_shieldsConfigProvider.Configs[1]);
        _playerDataModel.AddInventoryEquipment(_shieldsConfigProvider.Configs[2]);
        _playerDataModel.AddInventoryEquipment(_shieldsConfigProvider.Configs[3]);
    }
}
