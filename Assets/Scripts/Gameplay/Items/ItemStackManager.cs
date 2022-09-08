using System.Linq;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class ItemStackManager : IPersistableData
{
    [Inject(Id = InjectId.allItems)] ItemObjectList _allItems;
    [Inject(Id = InjectId.prefab)] Item _itemPrefab;
    [Inject(Id = InjectId.prefab)] ItemStack _itemStackPrefab;

    static readonly Collider2D[] _results = new Collider2D[32];

    public static ItemStack FindAt(Vector2 position)
    {
        var roundedPosition = Vector2Int.RoundToInt(position);
        var size = Physics2D.OverlapPointNonAlloc(roundedPosition, _results);

        for (var i = 0; i < size; ++i)
        {
            if (_results[i].TryGetComponent<ItemStack>(out var itemStack))
                return itemStack;
        }

        return null;
    }

    public ItemStack FindOrCreateAt(Vector2 position)
    {
        return FindAt(position) ?? CreateAt(position);
    }

    ItemStack CreateAt(Vector2 position)
    {
        var roundedPosition = Vector2Int.RoundToInt(position);

        return Object.Instantiate(
            _itemStackPrefab,
            roundedPosition.ToVector3(),
            Quaternion.identity
        );
    }

    public void Load(PersistentData data)
    {
        foreach (var stack in Object.FindObjectsOfType<ItemStack>())
        {
            Utilities.DestroyGameObject(stack.gameObject);
        }

        foreach (var stackData in data.stacks)
        {
            var stack = Object.Instantiate(
                _itemStackPrefab,
                stackData.position,
                Quaternion.identity
            );

            foreach (var itemObjectName in stackData.itemObjectNames)
            {
                var itemObject = _allItems.FindByName(itemObjectName);

                if (itemObject == null)
                {
                    Debug.LogError($"Could not find item {itemObjectName}");
                    return;
                }

                var item = Object.Instantiate(_itemPrefab);
                item.Initialize(itemObject);
                stack.Push(item, false);
            }
        }
    }

    public void Save(PersistentData data)
    {
        var stacks = Object.FindObjectsOfType<ItemStack>();
        data.stacks = new ItemStack.Data[stacks.Length];

        data.stacks = stacks.Select(
            stack => new ItemStack.Data
            {
                position = stack.transform.position,
                itemObjectNames = stack.items.Select(item => item.itemObject.name).ToArray(),
            }
        ).ToArray();
    }
}