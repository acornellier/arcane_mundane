using System;
using MoreLinq;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class ItemStackManager : IDataPersistence
{
    [Inject] Settings _settings;

    static readonly Collider2D[] _results = new Collider2D[32];

    public static ItemStack FindAt(Vector3 position)
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

    public ItemStack FindOrCreateAt(Vector3 position)
    {
        return FindAt(position) ?? CreateAt(position);
    }

    ItemStack CreateAt(Vector3 position)
    {
        var roundedPosition = Vector2Int.RoundToInt(position);

        return Object.Instantiate(
            _settings.itemStackPrefab,
            roundedPosition.ToVector3(),
            Quaternion.identity
        );
    }

    public void Load(PersistentData data)
    {
        var stacks = Object.FindObjectsOfType<ItemStack>();
        foreach (var stack in stacks)
        {
            Object.Destroy(stack);
        }

        foreach (var stackData in data.stacks)
        {
            var position = PersistentData.ArrayToVector3(stackData.position);
            var stack = Object.Instantiate(
                _settings.itemStackPrefab,
                position,
                Quaternion.identity
            );

            foreach (var itemObjectName in stackData.itemObjectNames)
            {
                var itemObject = _settings.allItems.FindByName(itemObjectName);

                if (itemObject == null)
                {
                    Debug.LogError($"Could not find item {itemObjectName}");
                    return;
                }

                var item = Object.Instantiate(_settings.itemPrefab);
                item.Initialize(itemObject);
                stack.Push(item);
            }
        }
    }

    public void Save(ref PersistentData data)
    {
        var stacks = Object.FindObjectsOfType<ItemStack>();
        data.stacks = new ItemStack.Data[stacks.Length];

        foreach (var (stackIdx, stack) in stacks.Index())
        {
            var stackData = data.stacks[stackIdx];
            stackData.position = PersistentData.Vector3ToArr(stack.transform.position);
            stackData.itemObjectNames = new string[stack.items.Count];
            foreach (var (itemIdx, item) in stack.items.Index())
            {
                stackData.itemObjectNames[itemIdx] = item.itemObject.name;
            }
        }
    }

    [Serializable]
    public class Settings
    {
        public ItemStack itemStackPrefab;
        public Item itemPrefab;
        public ItemObjectList allItems;
    }
}