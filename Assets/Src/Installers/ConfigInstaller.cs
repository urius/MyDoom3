using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ConfigInstaller", menuName = "Installers/ConfigInstaller")]
public class ConfigInstaller : ScriptableObjectInstaller<ConfigInstaller>
{
    [SerializeField]
    private ShipsConfigProvider _shipsConfigProvider;
    [SerializeField]
    private WeaponsConfigProvider _bulletsConfigProvider;

    public override void InstallBindings()
    {
        Container.BindInstance(_shipsConfigProvider);
        Container.BindInstance(_bulletsConfigProvider);
    }
}