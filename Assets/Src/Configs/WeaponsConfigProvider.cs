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
}

public enum WeaponId
{
    RedLaserLite,
    RedLaserMedium,
}

public enum BulletType
{
    Laser,
}

[Serializable]
public class WeaponConfig
{
    public WeaponId WeaponId;
    public BulletType Type;
    public float BulletSpeed;
    public int Damage;
    public int CooldownFrames;
    public GameObject BulletPrefab;
    public GameObject SparksPrefab;
}