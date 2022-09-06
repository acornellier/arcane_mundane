using System;
using System.Collections;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class DragonDrop : MonoBehaviour
{
    [SerializeField] ItemObjectList _allItems;
    [SerializeField] Item _itemPrefab;
    [SerializeField] int _dropCount = 10;
    [SerializeField] float _timeBetweenDrops = 0.4f;

    [Inject] GameManager _gameManager;
    [Inject] ItemStackManager _itemStackManager;

    void OnEnable()
    {
        _gameManager.OnGamePhaseChange += HandleGamePhaseChange;
    }

    void OnDisable()
    {
        _gameManager.OnGamePhaseChange -= HandleGamePhaseChange;
    }

    void HandleGamePhaseChange(GamePhase oldPhase, GamePhase newPhase)
    {
        if (newPhase == GamePhase.Delivery)
            StartCoroutine(CO_DropOff());
    }

    IEnumerator CO_DropOff()
    {
        var dropsDone = 0;
        var itemIdx = 0;
        while (dropsDone < _dropCount && itemIdx < _allItems.items.Count)
        {
            var itemObject = _allItems.items[itemIdx];
            var item = Instantiate(_itemPrefab);
            item.Initialize(itemObject);

            DropItem(item);

            dropsDone += 1;
            itemIdx += 1;
            yield return new WaitForSeconds(_timeBetweenDrops);
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