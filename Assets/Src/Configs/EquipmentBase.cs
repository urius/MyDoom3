using System;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Engine,
    Shield,
}

public abstract class EquipmentBase
{
    public abstract EquipmentType EquipmentType { get; }

    public GameObject Prefab;
    public Sprite IconSprite;
    public string NameId;
    public bool HideForPlayer;
    public int Cost;

    public abstract EquipmentMin ToEquipmentMin();
}

[Serializable]
public class EquipmentMin
{
    public EquipmentType EquipmentType;
    public int EquipmentId;

    public EquipmentMin(EquipmentType equipmentType, int equipmentId)
    {
        EquipmentType = equipmentType;
        EquipmentId = equipmentId;
    }
}
