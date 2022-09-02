using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour
{
    public ItemObject item;

    SpriteRenderer _spriteRenderer;

    static readonly int _outlineThickness = Shader.PropertyToID("_OutlineThickness");

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.material = new Material(_spriteRenderer.material);
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

    public void Highlight()
    {
        _spriteRenderer.material.SetFloat(_outlineThickness, 1);
    }

    public void Unhighlight()
    {
        if (_spriteRenderer)
            _spriteRenderer.material.SetFloat(_outlineThickness, 0);
    }

    void UpdateItemData()
    {
        if (!item) return;

        name = item.Name;
        GetComponent<SpriteRenderer>().sprite = item.sprite;
    }
}