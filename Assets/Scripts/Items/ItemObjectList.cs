using UnityEngine;

[CreateAssetMenu(fileName = "ItemObjectList", menuName = "Inventory/ItemObjectList", order = 0)]
public class ItemObjectList : ScriptableObject
{
    public ItemObject[] items;
}