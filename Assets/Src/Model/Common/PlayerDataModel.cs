using System;
using System.Collections.Generic;

public class PlayerDataModel
{
    public event Action<EquipmentBase> InventoryEquipmentRemoved = delegate { };
    public event Action<int, EquipmentBase> InventoryEquipmentSet = delegate { };
    public int Money;
    public int Exp;
    public ShipData ShipData;

    private List<EquipmentBase> _inventoryEqipments = new List<EquipmentBase>();

    public PlayerDataModel()
    {
    }

    public ShipConfig ShipConfig => ShipData.ShipConfig;

    public void AddInventoryEquipment(EquipmentBase equipment)
    {
        _inventoryEqipments.Add(equipment);

        InventoryEquipmentSet(_inventoryEqipments.Count - 1, equipment);
    }

    public void RemoveInventoryEquipment(EquipmentBase equipment)
    {
        _inventoryEqipments.Remove(equipment);

        InventoryEquipmentRemoved(equipment);
    }

    public void SwapInventoryEquipment(EquipmentBase equipmentInInventoryToRemove, EquipmentBase newEquipment)
    {
        var index = _inventoryEqipments.IndexOf(equipmentInInventoryToRemove);
        _inventoryEqipments[index] = newEquipment;

        InventoryEquipmentSet(index, newEquipment);
    }

    public bool HaveEquipmentInInventory(EquipmentBase equipment)
    {
        return _inventoryEqipments.IndexOf(equipment) > -1;
    }

    public IReadOnlyList<EquipmentBase> InventoryEqipments => _inventoryEqipments;
}
