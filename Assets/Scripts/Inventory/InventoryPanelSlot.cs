using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventoryPanelSlot : MonoBehaviour
{
    public InventorySlot slot;

    Image _image;

    void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void Initialize(InventorySlot inventorySlot, int row, int column)
    {
        slot = inventorySlot;
        _image.sprite = slot.item.sprite;
        _image.rectTransform.anchoredPosition = new Vector2(
            column * _image.rectTransform.rect.width,
            -1 * row * _image.rectTransform.rect.height
        );
    }
}