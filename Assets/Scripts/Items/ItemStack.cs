using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemStack : Interactable
{
    [SerializeField] float _spacing = 0.25f;

    public bool isFull => _items.Count >= _maxSize;

    Player _player;

    int _maxSize = 3;
    bool _isHighlighted;
    readonly Stack<Item> _items = new();

    void Awake()
    {
        _player = FindObjectOfType<Player>();
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

        if (!_items.Any())
        {
            Destroy(gameObject);
            return;
        }

        if (_isHighlighted)
            Highlight();
    }

    public override void Highlight()
    {
        _isHighlighted = true;
        if (_items.TryPeek(out var item))
            item.Highlight();
    }

    public override void Unhighlight()
    {
        _isHighlighted = false;
        if (_items.TryPeek(out var item))
            item.Unhighlight();
    }
}