using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D))]
public class Requester : MonoBehaviour
{
    public Action<ItemObject> onRequestComplete;

    Player _playerInRange;
    readonly List<ItemObject> _currentRequests = new();

    void Update()
    {
        if (!_playerInRange) return;

        CheckPlayerStack();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<Player>();
        if (!player) return;

        _playerInRange = player;
        CheckPlayerStack();
    }

    void OnTriggerExit2D(Collider2D col)
    {
        var player = col.GetComponent<Player>();
        if (!player) return;

        _playerInRange = null;
    }

    public ItemObject MakeRequest()
    {
        var item = FindUnrequestedItem();
        _currentRequests.Add(item);
        return item;
    }

    ItemObject FindUnrequestedItem()
    {
        var items = FindObjectsOfType<Item>();
        if (items.Length == 0)
            throw new Exception("No items available");

        for (var i = 0; i < 100; ++i)
        {
            var itemObject = items[Random.Range(0, items.Length)].itemObject;
            if (!_currentRequests.Contains(itemObject))
                return itemObject;
        }

        throw new Exception("Could not find anywhere to drop spawned item");
    }

    void CheckPlayerStack()
    {
        var debugSkip = Input.GetKey(KeyCode.G);
        if (debugSkip)
        {
            foreach (var request in _currentRequests)
            {
                onRequestComplete?.Invoke(request);
            }

            _currentRequests.Clear();
            return;
        }

        var topOfStack = _playerInRange.topOfStack;
        if (!topOfStack || !_currentRequests.Contains(topOfStack.itemObject))
            return;

        _playerInRange.DestroyTop();
        _currentRequests.Remove(topOfStack.itemObject);
        onRequestComplete?.Invoke(topOfStack.itemObject);
    }
}