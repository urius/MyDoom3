using System;
using System.Collections.Generic;
using System.Linq;

public class ShipData
{
    public event Action<int, EquipmentConfigBase> EquipmentSet = delegate { };
    public event Action<int, EquipmentConfigBase> EquipmentRemoved = delegate { };

    private readonly WeaponConfig[] _weaponsConfig;

    public readonly ShipConfig ShipConfig;

    public ShipData(ShipConfig config, WeaponConfig[] weaponsConfig)
    {
        ShipConfig = config;
        _weaponsConfig = weaponsConfig;
    }

    public ShipData(ShipConfig config, WeaponConfig[] weaponsConfig, EngineConfig engineConfig, ShieldConfig shieldConfig)
        :this(config, weaponsConfig)
    {
        EngineConfig = engineConfig;
        ShieldConfig = shieldConfig;
    }

    public IReadOnlyList<WeaponConfig> WeaponsConfig => _weaponsConfig;
    public ShieldConfig ShieldConfig { get; private set; }
    public EngineConfig EngineConfig { get; private set; }

    public void SetupEquipment(int slotIndex, EquipmentConfigBase equipment)
    {
        switch (equipment.EquipmentType)
        {
            case EquipmentType.Weapon:
                _weaponsConfig[slotIndex] = (WeaponConfig)equipment;
                break;
            case EquipmentType.Shield:
                ShieldConfig = (ShieldConfig)equipment;
                break;
            case EquipmentType.Engine:
                EngineConfig = (EngineConfig)equipment;
                break;
        }
        EquipmentSet(slotIndex, equipment);
    }

    public EquipmentConfigBase GetEquipment(EquipmentType type, int index)
    {
        return GetEquipmentsByType(type)[index];
    }

    public bool IsEquipped(EquipmentConfigBase equipment)
    {
        return GetEquipmentsByType(equipment.EquipmentType).Any(e => e == equipment);
    }

    public int GetEquipmentSlotIndex(EquipmentConfigBase equipment)
    {
        var equpmentsArray = GetEquipmentsByType(equipment.EquipmentType);
        return Array.IndexOf(equpmentsArray, equipment);
    }

    public bool RemoveEquipment(EquipmentConfigBase equipment)
    {
        var index = GetEquipmentSlotIndex(equipment);

        if (index != -1)
        {
            switch (equipment.EquipmentType)
            {
                case EquipmentType.Weapon:
                    _weaponsConfig[index] = null;
                    break;
                case EquipmentType.Shield:
                    ShieldConfig = null;
                    break;
                case EquipmentType.Engine:
                    EngineConfig = null;
                    break;
            }

            EquipmentRemoved(index, equipment);
            return true;
        }

        return false;
    }

    public ShipDataMin ToShipDataMin()
    {
        return new ShipDataMin(
            ShipConfig.ShipType,
            _weaponsConfig.Select(w => w.WeaponId).ToArray(),
            ShieldConfig.ShieldId,
            EngineConfig.EngineId);
    }

    private EquipmentConfigBase[] GetEquipmentsByType(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Weapon:
                return _weaponsConfig;
            case EquipmentType.Shield:
                return new EquipmentConfigBase[] { ShieldConfig };
            case EquipmentType.Engine:
                return new EquipmentConfigBase[] { EngineConfig };
        }

        return new EquipmentConfigBase[0];
    }
}
