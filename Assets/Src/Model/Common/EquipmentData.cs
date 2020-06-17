using UnityEngine;

public class EquipmentData
{
    public readonly EquipmentConfigBase Config;

    public EquipmentData(EquipmentConfigBase config)
    {
        Config = config;
    }

    public EquipmentMin ToEquipmentMin() => Config.ToEquipmentMin();
    public EquipmentType EquipmentType => Config.EquipmentType;
    public Sprite IconSprite => Config.IconSprite;
}

public class EquipmentData<TConfig> : EquipmentData
    where TConfig : EquipmentConfigBase
{
    public EquipmentData(TConfig config)
        : base(config)
    {
    }

    public new TConfig Config => (TConfig)base.Config;
}

public class WeaponData : EquipmentData<WeaponConfig>
{
    public WeaponData(WeaponConfig weaponConfig) : base(weaponConfig) { }

    public WeaponId WeaponId => Config.WeaponId;
}

public class ShieldData : EquipmentData<ShieldConfig>
{
    public ShieldData(ShieldConfig config) : base(config) { }

    public ShieldId ShieldId => Config.ShieldId;
}

public class EngineData : EquipmentData<EngineConfig>
{
    public EngineData(EngineConfig config) : base(config) { }

    public EngineId EngineId => Config.EngineId;
}
