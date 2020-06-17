using System.Linq;
using Zenject;

public class EquipmentShopConfigsProvider
{
    private WeaponsConfigProvider _weaponsConfigProvider;
    private EnginesConfigProvider _enginesConfigProvider;
    private ShieldsConfigProvider _shieldsConfigProvider;

    [Inject]
    public void Construct(
        WeaponsConfigProvider weaponsConfigProvider,
        EnginesConfigProvider enginesConfigProvider,
        ShieldsConfigProvider shieldsConfigProvider)
    {
        _weaponsConfigProvider = weaponsConfigProvider;
        _enginesConfigProvider = enginesConfigProvider;
        _shieldsConfigProvider = shieldsConfigProvider;
    }

    public WeaponConfig[] GetWeapons()
    {
        return _weaponsConfigProvider.Configs.Where(FilterCongigs).ToArray();
    }

    public EngineConfig[] GetEngines()
    {
        return _enginesConfigProvider.Configs.Where(FilterCongigs).ToArray();
    }

    public ShieldConfig[] GetShields()
    {
        return _shieldsConfigProvider.Configs.Where(FilterCongigs).ToArray();
    }

    private bool FilterCongigs(EquipmentConfigBase config)
    {
        return config.HideForPlayer == false;
    }
}
