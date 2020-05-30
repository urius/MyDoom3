using UnityEngine;
using Zenject;

public class MenuSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInstance(new MenuEventsAggregator());
        Container.BindInterfacesAndSelfTo<BuySellItemController>().AsSingle();
    }
}