using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemObjectList", menuName = "Inventory/ItemObjectList", order = 0)]
public class ItemObjectList : ScriptableObject
{
    public List<ItemObject> items;

    public ItemObject FindByName(string itemName)
    {
        return items.Find(itemObject => itemObject.name == itemName);
    }
}