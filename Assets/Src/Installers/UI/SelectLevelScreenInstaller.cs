using Zenject;

public class SelectLevelScreenInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<SelectLevelScreenController>().AsSingle();
    }
}