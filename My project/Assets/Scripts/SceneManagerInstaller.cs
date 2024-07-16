using Zenject;
using UnityEngine;

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
    }
}
