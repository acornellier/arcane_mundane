using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ItemStack : Interactable
{
    [SerializeField] float _spacing = 0.25f;

    public bool isFull => _items.Count >= _maxSize;

    Player _player;
    SpriteRenderer _spriteRenderer;

    static readonly Collider2D[] _results = new Collider2D[32];

    int _maxSize = 3;
    bool _isHighlighted;
    readonly Stack<Item> _items = new();

    void Awake()
    {
        _player = FindObjectOfType<Player>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _items.Clear();
        var childrenItems = GetComponentsInChildren<Item>();
        _maxSize = Mathf.Max(_maxSize, childrenItems.Length);
        foreach (var item in childrenItems)
        {
            Push(item);
        }
    }

    public void Push(Item item)
    {
        if (isFull) return;

        item.transform.SetParent(transform, false);
        item.transform.localPosition = Vector3.zero + _items.Count * _spacing * Vector3.up;
        item.GetComponent<SpriteRenderer>().sortingOrder = _items.Count;

        if (_isHighlighted)
        {
            item.Highlight();
            if (_items.Any())
                _items.Peek().Unhighlight();
        }

        _items.Push(item);
    }

    public override void Interact()
    {
        if (!_items.Any() || !_player.canPickUpItem) return;

        var item = _items.Pop();
        item.Unhighlight();
        _player.PickUpItem(item);

        if (_isHighlighted)
            Highlight();
    }

    public override void Highlight()
    {
        _isHighlighted = true;
        _spriteRenderer.enabled = true;
        if (_items.TryPeek(out var item))
            item.Highlight();
    }

    public override void Unhighlight()
    {
        _isHighlighted = false;
        _spriteRenderer.enabled = false;
        if (_items.TryPeek(out var item))
            item.Unhighlight();
    }

    public static ItemStack FindAt(Vector3 position)
    {
        var size = Physics2D.OverlapPointNonAlloc(position, _results);

        for (var i = 0; i < size; ++i)
        {
            if (_results[i].TryGetComponent<ItemStack>(out var itemStack))
                return itemStack;
        }

        return null;
    }
}