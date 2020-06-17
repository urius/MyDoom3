using System;
using System.Linq;
using Zenject;

public class ModelsFactory
{
    private readonly ShipsConfigProvider _shipsConfigProvider;
    private readonly WeaponsConfigProvider _weaponsConfigProvider;
    private readonly ShieldsConfigProvider _shieldsConfigProvider;
    private readonly EnginesConfigProvider _enginesConfigProvider;
    private readonly LevelsConfigProvider _levelsConfigProvider;

    [Inject]
    public ModelsFactory(
        ShipsConfigProvider shipsConfigProvider,
        WeaponsConfigProvider weaponsConfigProvider,
        ShieldsConfigProvider shieldsConfigProvider,
        EnginesConfigProvider enginesConfigProvider,
        LevelsConfigProvider levelsConfigProvider)
    {
        _shipsConfigProvider = shipsConfigProvider;
        _weaponsConfigProvider = weaponsConfigProvider;
        _shieldsConfigProvider = shieldsConfigProvider;
        _enginesConfigProvider = enginesConfigProvider;
        _levelsConfigProvider = levelsConfigProvider;
    }

    public ShipData CreateShipData(ShipDataMin shipDataMin)
    {
        var shipConfig = _shipsConfigProvider.GetConfigByShipType(shipDataMin.ShipType);
        var weaponsData = shipDataMin.WeaponIds.Select(_weaponsConfigProvider.GetConfigByWeaponId)
            .Select(c => (WeaponData)CreateEquipment(c))
            .ToArray();
        var engineData = (EngineData)CreateEquipment(_enginesConfigProvider.GetConfigByEngineId(shipDataMin.EngineId));
        var shieldData = (ShieldData)CreateEquipment(_shieldsConfigProvider.GetConfigByShieldId(shipDataMin.ShieldId));
        var shipData = new ShipData(
            shipConfig,
            weaponsData,
            engineData,
            shieldData);

        return shipData;
    }

    public EquipmentData CreateEquipment(EquipmentConfigBase config)
    {
        switch (config.EquipmentType)
        {
            case EquipmentType.Weapon:
                return new WeaponData((WeaponConfig)config);
            case EquipmentType.Shield:
                return new ShieldData((ShieldConfig)config);
            case EquipmentType.Engine:
                return new EngineData((EngineConfig)config);
            default:
                throw new NotSupportedException($"Equipment type {config.EquipmentType} is not supported");
        }
    }

    public EquipmentData CreateEquipment(EquipmentMin equipmentMin)
    {
        switch (equipmentMin.EquipmentType)
        {
            case EquipmentType.Weapon:
                return CreateEquipment(_weaponsConfigProvider.GetConfigByWeaponId((WeaponId)equipmentMin.EquipmentId));
            case EquipmentType.Shield:
                return CreateEquipment(_shieldsConfigProvider.GetConfigByShieldId((ShieldId)equipmentMin.EquipmentId));
            case EquipmentType.Engine:
                return CreateEquipment(_enginesConfigProvider.GetConfigByEngineId((EngineId)equipmentMin.EquipmentId));
            default:
                throw new NotImplementedException($"Equipment type {equipmentMin.EquipmentType} is not supported");
        }
    }

    internal LevelData CreateLevel(LevelDataMin levelDataMin)
    {
        var config = _levelsConfigProvider.LevelConfigs[levelDataMin.Index];
        return new LevelData(levelDataMin, config);
    }
}
