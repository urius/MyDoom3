using System;
using System.Collections.Generic;
using System.Linq;

public class ShipData
{
    public event Action<int, EquipmentBase> EquipmentSet = delegate { };
    public event Action<int, EquipmentBase> EquipmentRemoved = delegate { };
    
    public readonly ShipConfig ShipConfig;
    public readonly WeaponConfig[] _weaponsConfig;

    public ShipData(ShipConfig config, WeaponConfig[] weaponsConfig)
    {
        ShipConfig = config;
        _weaponsConfig = weaponsConfig;
    }

    public IReadOnlyList<WeaponConfig> WeaponsConfig => _weaponsConfig;
    public ShieldConfig ShieldConfig { get; private set; }

    public void SetupEquipment(int slotIndex, EquipmentBase equipment)
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
                throw new NotImplementedException("EquipmentType.Engine is not implemented yet");
        }
        EquipmentSet(0, equipment);
    }

    public EquipmentBase GetEquipment(EquipmentType type, int index)
    {
        return GetEquipmentsByType(type)[index];
    }

    public bool IsEquipped(EquipmentBase equipment)
    {
        return GetEquipmentsByType(equipment.EquipmentType).Any(e => e == equipment);
    }

    public int GetEquipmentSlotIndex(EquipmentBase equipment)
    {
        var equpmentsArray = GetEquipmentsByType(equipment.EquipmentType);
        return Array.IndexOf(equpmentsArray, equipment);
    }

    public bool RemoveEquipment(EquipmentBase equipment)
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
                    throw new NotImplementedException("EquipmentType.Engine is not implemented yet");
            }

            EquipmentRemoved(index, equipment);
            return true;
        }

        return false;
    }

    private EquipmentBase[] GetEquipmentsByType(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Weapon:
                return _weaponsConfig;
            case EquipmentType.Shield:
                return new EquipmentBase[] { ShieldConfig };
            case EquipmentType.Engine:
                throw new NotImplementedException("EquipmentType.Engine is not implemented yet");
        }

        return new EquipmentBase[0];
    }
}
