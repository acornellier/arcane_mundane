﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ItemStack : Interactable
{
    [SerializeField] float _spacing = 0.25f;
    [SerializeField] int _maxSize = 3;
    [SerializeField] float _underneathAlpha = 0.7f;

    public bool isFull => _items.Count >= _maxSize;

    Player _player;
    Collider2D _collider;
    SpriteRenderer _renderer;

    static readonly Collider2D[] _results = new Collider2D[32];

    bool _isHighlighted;
    readonly Stack<Item> _items = new();

    void Awake()
    {
        _player = FindObjectOfType<Player>();
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
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

        item.transform.SetParent(transform);
        item.MoveToLocal(Vector2.zero + _items.Count * _spacing * Vector2.up);
        item.GetComponent<SpriteRenderer>().sortingOrder = _items.Count;

        if (_isHighlighted)
        {
            item.Highlight();
            if (_items.Any())
                _items.Peek().Unhighlight();
        }

        _collider.isTrigger = false;

        _items.Push(item);

        SetAlpha();
    }

    public override void Interact()
    {
        if (_items.IsEmpty() || !_player.canPickUpItem) return;

        var item = _items.Pop();
        item.Unhighlight();
        _player.PickUpItem(item);

        if (_items.IsEmpty())
            _collider.isTrigger = true;

        if (_isHighlighted)
            Highlight();

        SetAlpha();
    }

    public override void Highlight()
    {
        _isHighlighted = true;
        _renderer.enabled = true;
        if (_items.TryPeek(out var item))
            item.Highlight();
    }

    public override void Unhighlight()
    {
        _isHighlighted = false;
        _renderer.enabled = false;
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

    void SetAlpha()
    {
        var first = true;
        foreach (var item in _items)
        {
            var spriteRenderer = item.GetComponent<SpriteRenderer>();
            var color = spriteRenderer.color;
            color.a = first ? 1 : _underneathAlpha;
            spriteRenderer.color = color;
            first = false;
        }
    }
}