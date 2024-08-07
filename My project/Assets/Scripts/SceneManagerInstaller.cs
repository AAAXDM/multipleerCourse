using Zenject;
using UnityEngine;
using NetworkShared;

public class SceneManagerInstaller : MonoInstaller<SceneManagerInstaller>
{
    [SerializeField] NetworkingClientSettings settings;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<NetworkingClient>()
                 .AsSingle()
                 .WithArguments(settings)
                 .NonLazy();

        Container.BindInterfacesAndSelfTo<GameManager>()
                 .AsSingle()
                 .NonLazy();
        Container.BindInterfacesAndSelfTo<OnAuthHandler>().AsTransient();
        Container.BindInterfacesAndSelfTo<OnAuthFailedHandler>().AsTransient();
        Container.BindInterfacesAndSelfTo<OnServerStatusRequestHandler>().AsTransient();
        Container.BindInterfacesAndSelfTo<OnStartGameHandler>().AsTransient();
        Container.BindInterfacesAndSelfTo<OnMarkCellHandler>().AsTransient();
        Container.Bind<Factory>().AsSingle().NonLazy();
    }
}
