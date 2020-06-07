using System;
using UnityEngine;

public class MenuEventsAggregator
{
    //main navigation
    
    public Action<GameObject> HomeClicked = delegate { };
    public Action<GameObject> ShipsClicked = delegate { };
    public Action<GameObject> EquipmentShopClicked = delegate { };
    public Action<GameObject> InventoryClicked = delegate { };

    //Equipment shop
    public Action<EquipmentBase> BuyEquipmentClicked = delegate { };

    //Ship shop
    public Action<ShipConfig> BuyShipClicked = delegate { };

    //Hangar screen
    public Action<EquipmentBase> EquipmentSlotMouseDown = delegate { };
    public Action<EquipmentBase, Vector3> FlyingEquipmentMouseUp = delegate { };
    public Action<int, EquipmentBase> FlyingEquipmentDropOverShip = delegate { };
    public Action<EquipmentBase> FlyingEquipmentDropOverInventory = delegate { };    
}
