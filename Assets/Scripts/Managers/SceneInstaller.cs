using Zenject;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();

        Container.Bind<Player>().FromComponentInHierarchy().AsSingle();
    }
}