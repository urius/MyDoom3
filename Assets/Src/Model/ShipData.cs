public class ShipData
{
    public readonly ShipConfig ShipConfig;
    public readonly WeaponConfig[] WeaponsConfig;

    public ShipData(ShipConfig config, WeaponConfig[] weaponsConfig)
    {
        ShipConfig = config;
        WeaponsConfig = weaponsConfig;
    }
}
