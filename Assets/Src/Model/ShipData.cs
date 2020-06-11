using System;
using System.Collections.Generic;
using System.Linq;

public class ShipData
{
    public event Action<int, EquipmentData> EquipmentSet = delegate { };
    public event Action<int, EquipmentData> EquipmentRemoved = delegate { };

    private readonly WeaponData[] _weaponsData;

    public readonly ShipConfig ShipConfig;

    public ShipData(ShipConfig config, WeaponData[] weaponsData)
    {
        ShipConfig = config;
        _weaponsData = weaponsData;
    }

    public ShipData(ShipConfig config, WeaponData[] weaponsData, EngineData engineData, ShieldData shieldData)
        :this(config, weaponsData)
    {
        EngineData = engineData;
        ShieldData = shieldData;
    }

    public IReadOnlyList<WeaponData> WeaponsData => _weaponsData;
    public ShieldData ShieldData { get; private set; }
    public EngineData EngineData { get; private set; }

    public void SetupEquipment(int slotIndex, EquipmentData equipment)
    {
        switch (equipment.EquipmentType)
        {
            case EquipmentType.Weapon:
                _weaponsData[slotIndex] = (WeaponData)equipment;
                break;
            case EquipmentType.Shield:
                ShieldData = (ShieldData)equipment;
                break;
            case EquipmentType.Engine:
                EngineData = (EngineData)equipment;
                break;
        }
        EquipmentSet(slotIndex, equipment);
    }

    public EquipmentData GetEquipment(EquipmentType type, int index)
    {
        return GetEquipmentsByType(type)[index];
    }

    public bool IsEquipped(EquipmentData equipment)
    {
        return GetEquipmentsByType(equipment.EquipmentType).Any(e => e == equipment);
    }

    public int GetEquipmentSlotIndex(EquipmentData equipment)
    {
        var equpmentsArray = GetEquipmentsByType(equipment.EquipmentType);
        return Array.IndexOf(equpmentsArray, equipment);
    }

    public bool RemoveEquipment(EquipmentData equipment)
    {
        var index = GetEquipmentSlotIndex(equipment);

        if (index != -1)
        {
            switch (equipment.EquipmentType)
            {
                case EquipmentType.Weapon:
                    _weaponsData[index] = null;
                    break;
                case EquipmentType.Shield:
                    ShieldData = null;
                    break;
                case EquipmentType.Engine:
                    EngineData = null;
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
            _weaponsData.Select(w => w.WeaponId).ToArray(),
            ShieldData.ShieldId,
            EngineData.EngineId);
    }

    private EquipmentData[] GetEquipmentsByType(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Weapon:
                return _weaponsData;
            case EquipmentType.Shield:
                return new EquipmentData[] { ShieldData };
            case EquipmentType.Engine:
                return new EquipmentData[] { EngineData };
        }

        return new EquipmentData[0];
    }
}
