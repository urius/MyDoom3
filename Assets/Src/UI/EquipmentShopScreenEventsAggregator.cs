using System;
using UnityEngine;

public class EquipmentShopScreenEventsAggregator : MonoBehaviour
{
    public Action<ShieldConfig> ShieldChosen = delegate { };
}
