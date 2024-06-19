using Zenject;

public class SceneManagerInstaller : MonoInstaller<SceneManagerInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<NetworkingClient>()
             .FromComponentInHierarchy()
             .AsSingle()
             .NonLazy();
    }
}
