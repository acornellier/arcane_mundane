using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] InventoryObject inventory;

    [SerializeField] Image itemImagePrefab;
    [SerializeField] int columns;
    [SerializeField] int rows;

    int _slotDimensions;

    readonly List<Image> _items = new();

    void Awake()
    {
        var dimensions = GetComponent<RectTransform>().rect.width / columns;
        _slotDimensions = (int)dimensions;
        if (Mathf.Abs(dimensions - _slotDimensions) > 0.001)
            Debug.LogError($"Dimensions not a round number: {dimensions}");
    }

    void Start()
    {
        inventory.onItemAdded += Draw;
        Draw();
    }

    void OnDisable()
    {
        inventory.onItemAdded -= Draw;
    }

    void Draw()
    {
        foreach (var item in _items)
        {
            Destroy(item.gameObject);
        }

        _items.Clear();

        var row = 0;
        var column = 0;

        foreach (var item in inventory.container)
        {
            if (column >= columns || row >= rows)
            {
                Debug.LogError("Inventory full");
                return;
            }

            var obj = Instantiate(itemImagePrefab, transform);
            obj.sprite = item.sprite;
            obj.rectTransform.anchoredPosition = new Vector2(
                column * _slotDimensions,
                row * _slotDimensions
            );
            _items.Add(obj);

            column += 1;
            if (column >= columns)
            {
                row += 1;
                column = 0;
            }
        }
    }
}