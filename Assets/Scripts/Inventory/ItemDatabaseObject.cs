using System.Collections.Generic;
using System.Linq;
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

    [ContextMenu("Update ID's")]
    public void UpdateID()
    {
        for (var i = 0; i < items.Length; i++)
        {
            if (items[i].data.Id != i)
                items[i].data.Id = i;
        }
    }

    public void OnBeforeSerialize()
    {
        ids = items.Select((item, index) => (item, index)).ToDictionary(
            x => x.item,
            x => x.index
        );
    }

    public void OnAfterDeserialize()
    {
    }
}