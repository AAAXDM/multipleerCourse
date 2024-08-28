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
        Container.BindInterfacesAndSelfTo<OnAuthHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<OnAuthFailedHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<OnServerStatusRequestHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<OnStartGameHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<OnMarkCellHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<FinishGameHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<OnNewRoundHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<OnFindFailedHandler>().AsSingle();
        Container.Bind<Factory>().AsSingle().NonLazy();
    }
}
