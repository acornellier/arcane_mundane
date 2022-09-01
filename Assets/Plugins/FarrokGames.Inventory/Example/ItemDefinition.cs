using UnityEngine;

namespace FarrokhGames.Inventory.Examples
{
    /// <summary>
    /// Scriptable Object representing an Inventory Item
    /// </summary>
    [CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 1)]
    public class ItemDefinition : ScriptableObject, IInventoryItem
    {
        [SerializeField] Sprite _sprite = null;
        [SerializeField] InventoryShape _shape = null;
        [SerializeField] ItemType _type = ItemType.Utility;
        [SerializeField] bool _canDrop = true;
        [SerializeField] [HideInInspector] Vector2Int _position = Vector2Int.zero;

        /// <summary>
        /// The name of the item
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string Name => name;

        /// <summary>
        /// The type of the item
        /// </summary>
        public ItemType type => _type;

        /// <inheritdoc />
        public Sprite sprite => _sprite;

        /// <inheritdoc />
        public int width => _shape.width;

        /// <inheritdoc />
        public int height => _shape.height;

        /// <inheritdoc />
        public Vector2Int position
        {
            get => _position;
            set => _position = value;
        }

        /// <inheritdoc />
        public bool IsPartOfShape(Vector2Int localPosition)
        {
            return _shape.IsPartOfShape(localPosition);
        }

        /// <inheritdoc />
        public bool canDrop => _canDrop;

        /// <summary>
        /// Creates a copy if this scriptable object
        /// </summary>
        public IInventoryItem CreateInstance()
        {
            var clone = Instantiate(this);
            clone.name = clone.name[..^7]; // Remove (Clone) from name
            return clone;
        }
    }
}