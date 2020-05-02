using UnityEngine;
using Zenject;

public class SceneContextInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        RegisterControllerFromComponent<SpawnController>();
        RegisterController<PlayerMovementController>();
        RegisterController<EnemyShipsController>();
        RegisterController<BulletsController>();
        RegisterController<CameraMoveControler>(); 

        Container.BindInterfacesAndSelfTo<UnitFactory>()
            .FromInstance(GetComponent<UnitFactory>());

        Container.BindInterfacesAndSelfTo<PlayerShipModelHolder>()
            .AsSingle();
        Container.BindInterfacesAndSelfTo<EnemyShipModelsProvider>()
            .AsSingle();
        Container.BindInterfacesAndSelfTo<BulletsModelProvider>()
            .AsSingle();
        Container.BindInterfacesAndSelfTo<ScreenBoundsProvider>()
            .AsSingle();
        Container.BindInterfacesAndSelfTo<EventsAggregator>()
            .AsSingle();
        Container.BindInterfacesAndSelfTo<ViewsManager>()
            .AsSingle();
        Container.BindInterfacesAndSelfTo<TickProvider>()
            .AsSingle();
    }

    private void RegisterControllerFromComponent<TController>()
        where TController : MonoBehaviour
    {
        Container.BindInterfacesTo<TController>()
            .FromInstance(GetComponent<TController>());
    }

    private void RegisterController<TController>()
    {
        Container.BindInterfacesTo<TController>()
            .AsSingle();
    }
}