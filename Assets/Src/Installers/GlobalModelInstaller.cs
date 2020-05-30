using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GlobalModelInstaller", menuName = "Installers/GlobalModelInstaller")]
public class GlobalModelInstaller : ScriptableObjectInstaller<GlobalModelInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerDataModel>().AsSingle();
    }
}