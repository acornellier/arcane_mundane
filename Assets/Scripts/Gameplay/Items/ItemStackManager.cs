using System;
using System.Linq;
using MoreLinq;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class ItemStackManager : IDataPersistence
{
    [Inject] Settings _settings;

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
            _settings.itemStackPrefab,
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
                position = PersistentData.Vector3ToArr(stack.transform.position),
                itemObjectNames = stack.items.Select(item => item.itemObject.name).ToArray(),
            }
        ).ToArray();
    }

    [Serializable]
    public class Settings
    {
        public ItemStack itemStackPrefab;
        public Item itemPrefab;
        public ItemObjectList allItems;
    }
}