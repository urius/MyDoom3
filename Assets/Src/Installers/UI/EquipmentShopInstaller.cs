using Zenject;

public class EquipmentShopInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<EquipmentShopConfigsProvider>().AsSingle();
    }
}