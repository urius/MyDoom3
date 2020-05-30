using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldsConfig", menuName = "Configs/ShieldsConfig")]
public class ShieldsConfigProvider : ScriptableObject
{
    [SerializeField]
    private ShieldConfig[] _configs;

    public ShieldConfig[] Configs => _configs;
}

public enum ShieldId
{
    BlueShieldSingle,
    BlueShieldDouble,
    GreenShieldSingle,
    GreenShieldDouble,
    ShieldGreenNuclear1,
    ShieldGreenNuclear2,
    ShieldGreenNuclear3,
}

[Serializable]
public class ShieldConfig : EquipmentBase
{
    public ShieldId ShieldId;
    public int ChargingSpeed;
    public int Capacity;
    public int Cost;

    public override EquipmentType EquipmentType => EquipmentType.Shield;
}
