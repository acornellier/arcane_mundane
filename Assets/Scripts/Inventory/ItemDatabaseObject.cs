using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "ItemDatabaseObject",
    menuName = "Inventory/ItemDatabaseObject",
    order = 0
)]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] ItemObject[] items;

    public Dictionary<ItemObject, int> ids = new();

    [ContextMenu("Update Ids")]
    public void UpdateIds()
    {
        for (var i = 0; i < items.Length; i++)
        {
            // items[i].id = i;
        }
    }

    public void OnBeforeSerialize()
    {
        UpdateIds();
    }

    public void OnAfterDeserialize()
    {
    }
}