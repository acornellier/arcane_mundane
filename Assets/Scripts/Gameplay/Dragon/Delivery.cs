using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MoreLinq;
using UnityEngine;
using Zenject;

public class Delivery : MonoBehaviour
{
    [SerializeField] float _timeBetweenDrops = 0.4f;

    [Inject] ItemStackManager _itemStackManager;
    [Inject(Id = InjectId.allItems)] ItemObjectList _allItems;
    [Inject(Id = InjectId.prefab)] Item _itemPrefab;

    public async UniTask Deliver(CancellationToken token, int numberOfItems)
    {
        var itemsDelivered = 0;

        var shuffledItems = _allItems.items.Shuffle();

        foreach (var itemObject in shuffledItems.Repeat())
        {
            if (itemsDelivered > numberOfItems)
                break;

            var item = Instantiate(_itemPrefab);
            item.Initialize(itemObject);

            DropItem(item);

            itemsDelivered += 1;
            await UniTask.Delay(TimeSpan.FromSeconds(_timeBetweenDrops), cancellationToken: token);
        }
    }

    void DropItem(Item item)
    {
        for (var ring = 0; ring < 5; ++ring)
        {
            for (var x = -ring; x <= ring; ++x)
            {
                for (var y = -ring; y <= ring; ++y)
                {
                    var position = (Vector2)transform.position + new Vector2(x, y);

                    var stack = _itemStackManager.FindOrCreateAt(position);
                    if (!stack.isFull)
                    {
                        item.transform.position = stack.transform.position + new Vector3(0, 2);
                        stack.Push(item);
                        return;
                    }
                }
            }
        }

        throw new Exception("Could not find anywhere to drop spawned item");
    }
}