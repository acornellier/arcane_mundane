using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
[RequireComponent(typeof(RectTransform))]
public class InventoryPanel : MonoBehaviour
{
    [SerializeField] InventoryObject inventory;
    [SerializeField] InventoryPanelSlot inventoryPanelSlotPrefab;

    RectTransform _rectTransform;

    readonly Dictionary<Vector2Int, InventoryPanelSlot> _panelSlots = new();

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        InitializeSize();
        Draw();
        inventory.onChange += Draw;
    }

    void OnDisable()
    {
        inventory.onChange -= Draw;
    }

    void InitializeSize()
    {
        var gridLayout = GetComponent<GridLayoutGroup>();
        _rectTransform.sizeDelta = new Vector2(
            inventory.Slots.GetLength(1) * gridLayout.cellSize.x,
            inventory.Slots.GetLength(0) * gridLayout.cellSize.y
        );

        for (var row = 0; row < inventory.Slots.GetLength(0); ++row)
        {
            for (var column = 0; column < inventory.Slots.GetLength(1); ++column)
            {
                var panelSlot = Instantiate(inventoryPanelSlotPrefab, transform);
                panelSlot.Initialize(this, inventory.Slots[row, column]);
                _panelSlots.Add(new Vector2Int(row, column), panelSlot);
            }
        }
    }

    void Draw()
    {
        for (var row = 0; row < inventory.Slots.GetLength(0); ++row)
        {
            for (var column = 0; column < inventory.Slots.GetLength(1); ++column)
            {
                _panelSlots[new Vector2Int(row, column)].Slot = inventory.Slots[row, column];
            }
        }
    }

    public void Swap(InventoryPanelSlot panelSlot1, InventoryPanelSlot panelSlot2)
    {
        inventory.SwapSlots(panelSlot1.Slot, panelSlot2.Slot);
    }
}