using System.Collections.Generic;

namespace FarrokhGames.Inventory.Examples
{
    public class InventoryProvider : IInventoryProvider
    {
        readonly List<IInventoryItem> _items = new();
        readonly int _maximumAlowedItemCount;
        ItemType _allowedItem;

        /// <summary>
        /// CTOR
        /// </summary>
        public InventoryProvider(int maximumAlowedItemCount = -1,
            ItemType allowedItem = ItemType.Any)
        {
            _maximumAlowedItemCount = maximumAlowedItemCount;
            _allowedItem = allowedItem;
        }

        public int inventoryItemCount => _items.Count;

        public bool isInventoryFull
        {
            get
            {
                if (_maximumAlowedItemCount < 0) return false;
                return inventoryItemCount >= _maximumAlowedItemCount;
            }
        }

        public bool AddInventoryItem(IInventoryItem item)
        {
            if (!_items.Contains(item))
            {
                _items.Add(item);
                return true;
            }

            return false;
        }

        public bool DropInventoryItem(IInventoryItem item)
        {
            return RemoveInventoryItem(item);
        }

        public IInventoryItem GetInventoryItem(int index)
        {
            return _items[index];
        }

        public bool CanAddInventoryItem(IInventoryItem item)
        {
            if (_allowedItem == ItemType.Any) return true;
            return ((ItemDefinition)item).type == _allowedItem;
        }

        public bool CanRemoveInventoryItem(IInventoryItem item)
        {
            return true;
        }

        public bool CanDropInventoryItem(IInventoryItem item)
        {
            return true;
        }

        public bool RemoveInventoryItem(IInventoryItem item)
        {
            return _items.Remove(item);
        }
    }
}