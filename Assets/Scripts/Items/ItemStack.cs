using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemStack : Interactable
{
    [SerializeField] float _spacing = 0.25f;
    [SerializeField] int _maxSize = 3;
    [SerializeField] float _underneathAlpha = 0.7f;

    public bool isFull => _items.Count >= _maxSize;

    Player _player;
    PlayerController _playerController;
    Collider2D _collider;

    static readonly Collider2D[] _results = new Collider2D[32];

    readonly Stack<Item> _items = new();

    void Awake()
    {
        _player = FindObjectOfType<Player>();
        _playerController = FindObjectOfType<PlayerController>();
        _collider = GetComponent<Collider2D>();
    }

    void OnEnable()
    {
        _playerController.onReveal += SetUnderneathAlpha;
    }

    void OnDisable()
    {
        _playerController.onReveal -= SetUnderneathAlpha;
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
        item.MoveTo((Vector2)transform.position + _items.Count * _spacing * Vector2.up);
        item.GetComponent<SpriteRenderer>().sortingOrder = _items.Count;

        _collider.isTrigger = false;

        _items.Push(item);
    }

    public override void Interact()
    {
        if (_items.IsEmpty() || !_player.canPickUpItem) return;

        var item = _items.Pop();
        _player.PickUpItem(item);

        if (_items.IsEmpty())
            _collider.isTrigger = true;
    }

    public override void Highlight()
    {
    }

    public override void Unhighlight()
    {
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

    void SetUnderneathAlpha(bool reveal)
    {
        var first = true;
        foreach (var item in _items)
        {
            var spriteRenderer = item.GetComponent<SpriteRenderer>();
            var color = spriteRenderer.color;
            color.a = first || !reveal ? 1 : _underneathAlpha;
            spriteRenderer.color = color;
            first = false;
        }
    }
}