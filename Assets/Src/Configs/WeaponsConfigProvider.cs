using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponsConfig", menuName = "Configs/WeaponsConfig")]
public class WeaponsConfigProvider : ScriptableObject
{
    [SerializeField]
    private WeaponConfig[] _configs;

    public WeaponConfig GetConfigByWeaponId(WeaponId weapon)
    {
        return _configs.First(c => c.WeaponId == weapon);
    }

    public WeaponConfig[] Configs => _configs;
}

public enum WeaponId
{
    Undefined,
    RedLaserMedium,
    GreenLaserMedium,
    BlueLaserMedium,
    YellowLaserMedium,
    GunMedium,
    MiniGunHeavy,
    RedLaserLite,
}

public enum BulletType
{
    StraightBullet,
}

[Serializable]
public class WeaponConfig : EquipmentBase
{
    public WeaponId WeaponId;
    public BulletType Type;
    public float BulletSpeed;
    public int Damage;
    public int CooldownFrames;
    public GameObject BulletPrefab;
    public GameObject SparksPrefab;

    public override EquipmentType EquipmentType => EquipmentType.Weapon;

    public override EquipmentMin ToEquipmentMin()
    {
        return new EquipmentMin(EquipmentType, (int)WeaponId);
    }
}