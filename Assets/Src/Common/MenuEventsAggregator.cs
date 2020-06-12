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
    public Action<EquipmentConfigBase> BuyEquipmentClicked = delegate { };

    //Ship shop
    public Action<ShipConfig> BuyShipClicked = delegate { };

    //Hangar screen
    public Action<EquipmentData> EquipmentSlotMouseDown = delegate { };
    public Action<EquipmentData, Vector3> FlyingEquipmentMouseUp = delegate { };
    public Action<EquipmentData> RequestSellEqipment = delegate { };    
    public Action MouseUp = delegate { };
    public Action<int, EquipmentData> FlyingEquipmentDropOverShip = delegate { };
    public Action<EquipmentData> FlyingEquipmentDropOverInventory = delegate { };    
}
