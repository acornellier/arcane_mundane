using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour, IInteractable
{
    public ItemObject item;

    SpriteRenderer _spriteRenderer;
    Player _player;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = FindObjectOfType<Player>();
        UpdateItemData();
    }

    void OnValidate()
    {
        UpdateItemData();
    }

    public void Initialize(ItemObject itemObject)
    {
        item = itemObject;
        UpdateItemData();
    }

    public void Interact()
    {
        if (_player.PickUpItem(item))
            Destroy(gameObject);
    }

    public void Highlight()
    {
        _spriteRenderer.color = Color.green;
    }

    public void Unhighlight()
    {
        _spriteRenderer.color = Color.white;
    }

    void UpdateItemData()
    {
        if (!item) return;

        name = item.Name;
        GetComponent<SpriteRenderer>().sprite = item.sprite;
    }
}