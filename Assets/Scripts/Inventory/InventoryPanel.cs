using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class InventoryPanel : MonoBehaviour
{
    [SerializeField] InventoryObject inventory;
    [SerializeField] InventoryPanelSlot itemImagePrefab;

    RectTransform _rectTransform;

    readonly Dictionary<Vector2Int, InventoryPanelSlot> _images = new();

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
        var imageRectTransform = itemImagePrefab.GetComponent<RectTransform>();
        _rectTransform.sizeDelta = new Vector2(
            inventory.Slots.GetLength(1) * imageRectTransform.rect.width,
            inventory.Slots.GetLength(0) * imageRectTransform.rect.height
        );
    }

    void Draw()
    {
        foreach (var image in _images.Values)
        {
            Destroy(image.gameObject);
        }

        _images.Clear();

        for (var row = 0; row < inventory.Slots.GetLength(0); ++row)
        {
            for (var column = 0; column < inventory.Slots.GetLength(1); ++column)
            {
                var slot = inventory.Slots[row, column];
                if (slot.IsEmpty) return;

                var panelSlot = Instantiate(itemImagePrefab, transform);
                panelSlot.Initialize(slot, row, column);
                _images.Add(new Vector2Int(row, column), panelSlot);
            }
        }
    }
}