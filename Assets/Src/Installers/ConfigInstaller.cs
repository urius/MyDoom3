using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ConfigInstaller", menuName = "Installers/ConfigInstaller")]
public class ConfigInstaller : ScriptableObjectInstaller<ConfigInstaller>
{
    [SerializeField] private LocalizationProvider _localizationProvider;
    [SerializeField] private ShipsConfigProvider _shipsConfigProvider;
    [SerializeField] private WeaponsConfigProvider _bulletsConfigProvider;
    [SerializeField] private ShieldsConfigProvider _shieldsConfigProvider;
    [SerializeField] private EnginesConfigProvider _enginesConfigProvider;
    [SerializeField] private LevelsConfigProvider _levelsConfigProvider;

    public override void InstallBindings()
    {
        Container.BindInstance(_localizationProvider);
        Container.BindInstance(_shipsConfigProvider);
        Container.BindInstance(_bulletsConfigProvider);
        Container.BindInstance(_shieldsConfigProvider);
        Container.BindInstance(_enginesConfigProvider);
        Container.BindInstance(_levelsConfigProvider);
    }
}