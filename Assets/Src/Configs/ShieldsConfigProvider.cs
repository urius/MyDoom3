using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldsConfig", menuName = "Configs/ShieldsConfig")]
public class ShieldsConfigProvider : ScriptableObject
{
    [SerializeField]
    private ShieldConfig[] _configs;

    public ShieldConfig[] Configs => _configs;

    public ShieldConfig GetConfigByShieldId(ShieldId shield)
    {
        return _configs.First(c => c.ShieldId == shield);
    }
}

public enum ShieldId
{
    Undefined,
    BlueShieldDouble,
    GreenShieldSingle,
    GreenShieldDouble,
    ShieldGreenNuclear1,
    ShieldGreenNuclear2,
    ShieldGreenNuclear3,
    BlueShieldSingle,
}

[Serializable]
public class ShieldConfig : EquipmentBase
{
    public ShieldId ShieldId;
    public int ChargingSpeed;
    public int Capacity;

    public override EquipmentType EquipmentType => EquipmentType.Shield;

    public override EquipmentMin ToEquipmentMin()
    {
        return new EquipmentMin(EquipmentType, (int)ShieldId);
    }
}
