using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryObject", menuName = "Inventory/InventoryObject", order = 0)]
public class InventoryObject : ScriptableObject
{
    public List<ItemObject> container = new();

    public Action onItemAdded;

    public void AddItem(ItemObject item)
    {
        container.Add(item);
        onItemAdded?.Invoke();
    }
}