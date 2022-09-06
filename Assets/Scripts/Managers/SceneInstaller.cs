using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] ItemStackManager.Settings itemStackManager;

    public override void InstallBindings()
    {
        Container.BindInstance(itemStackManager);

        Container.BindInterfacesAndSelfTo<Player>().FromComponentInHierarchy().AsSingle();
        Container.Bind<DialogueManager>().FromComponentInHierarchy().AsSingle();

        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<ItemStackManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<MenuManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<PersistentDataManager>().AsSingle();
    }
}