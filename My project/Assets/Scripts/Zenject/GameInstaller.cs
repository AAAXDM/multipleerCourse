using UnityEngine;
using Zenject;


public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameObject cellPrefab;

    public override void InstallBindings()
    {
        Container.Bind<CellGenerator>().FromComponentInHierarchy().AsSingle();
        Container.BindFactory<SceneCell, SceneCell.Factory>().FromComponentInNewPrefab(cellPrefab).AsSingle();
    }
}
