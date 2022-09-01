using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour
{
    [SerializeField] ItemStack stackPrefab;

    public ItemObject item;

    SpriteRenderer _spriteRenderer;

    static readonly Collider2D[] _results = new Collider2D[32];

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateItemData();
    }

    void OnValidate()
    {
        UpdateItemData();
    }

    public void DropAt(Vector3 position)
    {
        var stack = FindOrCreateStackAt(position);
        stack.Push(this);
    }

    public void Highlight()
    {
        _spriteRenderer.color = Color.green;
    }

    public void Unhighlight()
    {
        if (_spriteRenderer)
            _spriteRenderer.color = Color.white;
    }

    void UpdateItemData()
    {
        if (!item) return;

        name = item.Name;
        GetComponent<SpriteRenderer>().sprite = item.sprite;
    }

    ItemStack FindOrCreateStackAt(Vector3 position)
    {
        var size = Physics2D.OverlapPointNonAlloc(position, _results);

        for (var i = 0; i < size; ++i)
        {
            if (_results[i].TryGetComponent<ItemStack>(out var itemStack))
                return itemStack;
        }

        var newStack = Instantiate(stackPrefab, position, Quaternion.identity);
        return newStack;
    }
}