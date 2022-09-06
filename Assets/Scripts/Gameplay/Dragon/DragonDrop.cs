﻿using System;
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
        while (dropsDone < _dropCount)
        {
            var itemObject = _allItems.items[Random.Range(0, _allItems.items.Count)];
            var item = Instantiate(_itemPrefab);
            item.Initialize(itemObject);

            DropItem(item);

            dropsDone += 1;
            yield return new WaitForSeconds(_timeBetweenDrops);
        }
    }

    void DropItem(Item item)
    {
        for (var i = 0; i < 10; ++i)
        {
            var position = transform.position +
                           Random.Range(-2, 2) * Vector3.right +
                           Random.Range(-1, 1) * Vector3.up;

            var stack = _itemStackManager.FindOrCreateAt(position);
            if (stack.isFull)
                continue;

            item.transform.position = stack.transform.position + new Vector3(0, 2);
            stack.Push(item);
            return;
        }

        throw new Exception("Could not find anywhere to drop spawned item");
    }
}