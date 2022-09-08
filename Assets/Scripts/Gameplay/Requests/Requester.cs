using System;
using System.Collections.Generic;
using System.Linq;
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
        var items = FindObjectsOfType<Item>();
        if (items.Length == 0)
            throw new Exception("No items available");

        var item = items[Random.Range(0, items.Length)].itemObject;
        _currentRequests.Add(item);
        return item;
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