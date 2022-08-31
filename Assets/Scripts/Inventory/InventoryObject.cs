using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemObject item;
    public bool IsEmpty => item == null;
}

[CreateAssetMenu(fileName = "InventoryObject", menuName = "Inventory/InventoryObject", order = 0)]
public class InventoryObject : ScriptableObject
{
    [SerializeField] int rows = 4;
    [SerializeField] int columns = 4;

    InventorySlot[,] _slots;

    public InventorySlot[,] Slots
    {
        get
        {
            if (_slots == null) ResetSlots();
            return _slots;
        }
        private set => _slots = value;
    }

    public Action onChange;

    public void AddItem(ItemObject item)
    {
        var slot = FindEmptySlot();
        if (slot == null) return;

        slot.item = item;
        onChange?.Invoke();
    }

    InventorySlot FindEmptySlot()
    {
        return Slots.Cast<InventorySlot>().FirstOrDefault(slot => slot.IsEmpty);
    }

    public void ResetSlots()
    {
        Slots = new InventorySlot[rows, columns];
        for (var row = 0; row < Slots.GetLength(0); ++row)
        {
            for (var column = 0; column < Slots.GetLength(1); ++column)
            {
                Slots[row, column] = new InventorySlot();
            }
        }
    }
}