using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemObject item;
    public InventorySlot parent;
    public bool IsEmpty => item == null && parent == null;
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

    public void SwapSlots(InventorySlot slot1, InventorySlot slot2)
    {
        (slot1.item, slot2.item) = (slot2.item, slot1.item);
        onChange?.Invoke();
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