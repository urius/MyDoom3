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
}
