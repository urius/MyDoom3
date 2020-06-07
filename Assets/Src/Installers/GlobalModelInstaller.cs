using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GlobalModelInstaller", menuName = "Installers/GlobalModelInstaller")]
public class GlobalModelInstaller : ScriptableObjectInstaller<GlobalModelInstaller>
{
    [SerializeField]
    private DefaultPlayerDataProvider _defaultPlayerDataProvider;

    public override void InstallBindings()
    {
        Container.Bind<ModelsFactory>().AsTransient();

        Container.Bind<PlayerDataModel>().AsSingle();
        Container.BindInstance(_defaultPlayerDataProvider).AsSingle();
    }
}