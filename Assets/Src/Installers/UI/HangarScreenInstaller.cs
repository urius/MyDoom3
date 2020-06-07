using Zenject;

public class HangarScreenInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<HangarScreenController>().AsSingle();
    }
}