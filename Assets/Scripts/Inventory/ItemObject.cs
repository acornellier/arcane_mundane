using UnityEngine;

[CreateAssetMenu(fileName = "ItemObject", menuName = "Inventory/Item", order = 0)]
public class ItemObject : ScriptableObject
{
    public Sprite sprite;
    [TextArea(15, 20)] public string description;
}