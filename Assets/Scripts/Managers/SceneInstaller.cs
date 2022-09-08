using UnityEngine;
using Zenject;

public class InjectId
{
    public const string startQuest = "start";
    public const string allItems = "all";
    public const string prefab = "prefab";
}

public class SceneInstaller : MonoInstaller
{
    [SerializeField] QuestNode _startQuest;
    [SerializeField] ItemObjectList _allItems;
    [SerializeField] Item _itemPrefab;
    [SerializeField] ItemStack _itemStackPrefab;

    public override void InstallBindings()
    {
        Container.BindInstance(_startQuest).WithId(InjectId.startQuest);
        Container.BindInstance(_allItems).WithId(InjectId.allItems);
        Container.BindInstance(_itemPrefab).WithId(InjectId.prefab);
        Container.BindInstance(_itemStackPrefab).WithId(InjectId.prefab);

        Container.BindInterfacesAndSelfTo<Player>().FromComponentInHierarchy().AsSingle();
        Container.Bind<DialogueManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Delivery>().FromComponentInHierarchy().AsSingle();
        Container.Bind<MainTimer>().FromComponentInHierarchy().AsSingle();

        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<ItemStackManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<MenuManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<PersistentDataManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<QuestPersistence>().AsSingle();
    }
}