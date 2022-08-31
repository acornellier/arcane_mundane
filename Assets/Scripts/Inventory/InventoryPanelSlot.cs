using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPanelSlot : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler,
    IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image container;
    [SerializeField] Image icon;
    [SerializeField] Sprite containerHighlightedSprite;

    Sprite _containerDefaultSprite;

    InventoryPanel _inventoryPanel;
    InventorySlot _slot;

    Vector3 _mouseOffset;

    public InventorySlot Slot
    {
        get => _slot;
        set
        {
            _slot = value;
            UpdateImage();
        }
    }

    void Awake()
    {
        _containerDefaultSprite = container.sprite;
    }

    public void Initialize(InventoryPanel inventoryPanel, InventorySlot inventorySlot)
    {
        _inventoryPanel = inventoryPanel;
        Slot = inventorySlot;

        ResetPosition();
        UpdateImage();
    }

    void ResetPosition()
    {
        icon.rectTransform.anchoredPosition = Vector2.zero;
    }

    void UpdateImage()
    {
        if (Slot.item)
        {
            icon.sprite = Slot.item.sprite;
            icon.enabled = true;
        }
        else
        {
            icon.enabled = false;
        }
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!icon.enabled) return;

        icon.GetComponent<Canvas>().overrideSorting = true;
        icon.GetComponent<CanvasGroup>().blocksRaycasts = false;

        _mouseOffset = default;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                transform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out var globalMousePos
            ))
            _mouseOffset = icon.rectTransform.position - globalMousePos;

        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
    }

    void SetDraggedPosition(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                transform.parent as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out var globalMousePos
            ))
            icon.rectTransform.position = globalMousePos + _mouseOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        icon.GetComponent<Canvas>().overrideSorting = false;
        icon.GetComponent<CanvasGroup>().blocksRaycasts = true;
        ResetPosition();
    }

    public void OnDrop(PointerEventData eventData)
    {
        var originalObj = eventData.pointerDrag;
        if (originalObj == null) return;

        var panelSlot = originalObj.GetComponent<InventoryPanelSlot>();
        if (panelSlot == null) return;

        _inventoryPanel.Swap(this, panelSlot);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        container.sprite = containerHighlightedSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        container.sprite = _containerDefaultSprite;
    }
}