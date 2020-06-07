using UnityEngine;
using Zenject;

public class MenuSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInstance(new MenuEventsAggregator());

        Container.BindInterfacesAndSelfTo<DataInitializeController>().AsSingle();
        Container.BindInterfacesAndSelfTo<BuySellItemController>().AsSingle();
    }
}