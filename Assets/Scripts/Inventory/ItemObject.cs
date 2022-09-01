using UnityEngine;

[CreateAssetMenu(fileName = "ItemObject", menuName = "Inventory/Item", order = 0)]
public class ItemObject : ScriptableObject
{
    public Sprite sprite;
    public Vector2Int dimensions = Vector2Int.one;
    [TextArea(5, 20)] public string description;
}