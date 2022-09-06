using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class PlayerStack : MonoBehaviour
{
    [SerializeField] AudioSource pickUpSource;
    [SerializeField] AudioClip pickUpClip;

    [Inject] ItemStackManager _itemStackManager;

    const int _maxSize = 3;
    const float _spacing = 0.25f;

    public int count => _items.Count;

    readonly Stack<Item> _items = new();
    bool isFull => _items.Count >= _maxSize;
    bool _isHighlighted;

    void Start()
    {
        _items.Clear();
        foreach (var item in GetComponentsInChildren<Item>())
        {
            Push(item);
        }
    }

    void Push(Item item)
    {
        if (isFull) return;

        item.StopMoving();
        item.transform.SetParent(transform, false);
        item.transform.localPosition = Vector3.zero + _items.Count * _spacing * Vector3.up;
        item.GetComponent<SpriteRenderer>().sortingOrder = _items.Count;

        _items.Push(item);
    }

    public void PickUpAt(Vector2 position)
    {
        if (isFull) return;

        var stack = ItemStackManager.FindAt(position);
        if (!stack) return;

        Push(stack.Pop());
    }

    public void DropAt(Vector2 position)
    {
        if (_items.IsEmpty()) return;

        var stack = _itemStackManager.FindOrCreateAt(position);
        if (stack.isFull)
            return;

        stack.Push(_items.Pop());
    }

    public bool Any()
    {
        return _items.Any();
    }

    public Item Peek()
    {
        _items.TryPeek(out var item);
        return item;
    }

    public void DestroyTop()
    {
        if (_items.TryPop(out var item))
            // item.transform.SetParent(null);
            // item.MoveToLocal(Vector2.zero + _items.Count * _spacing * Vector2.up);
            Utilities.DestroyGameObject(item.gameObject);
    }
}