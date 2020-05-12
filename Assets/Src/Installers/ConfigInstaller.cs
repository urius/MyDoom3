using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ConfigInstaller", menuName = "Installers/ConfigInstaller")]
public class ConfigInstaller : ScriptableObjectInstaller<ConfigInstaller>
{
    [SerializeField]
    private ShipsConfigProvider _shipsConfigProvider;
    [SerializeField]
    private WeaponsConfigProvider _bulletsConfigProvider;
    [SerializeField]
    private LocalizationProvider _localizationProvider;

    public override void InstallBindings()
    {
        Container.BindInstance(_shipsConfigProvider);
        Container.BindInstance(_bulletsConfigProvider);
        Container.BindInstance(_localizationProvider);
    }
}