using System;
using UnityEngine;

public class EquipmentShopScreenEventsAggregator : MonoBehaviour
{
    public Action<EquipmentConfigBase> EquipmentChosen = delegate { };
}
