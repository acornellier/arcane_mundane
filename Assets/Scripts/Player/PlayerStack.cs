using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStack : MonoBehaviour
{
    [SerializeField] float spacing = 0.25f;

    public int count => _items.Count;

    bool _isHighlighted;
    readonly Stack<Item> _items = new();

    void Start()
    {
        _items.Clear();
        foreach (var item in GetComponentsInChildren<Item>())
        {
            Push(item);
        }
    }

    public void Push(Item item)
    {
        item.transform.SetParent(transform, false);
        item.transform.localPosition = Vector3.zero + _items.Count * spacing * Vector3.up;
        item.GetComponent<SpriteRenderer>().sortingOrder = _items.Count;

        _items.Push(item);
    }

    public bool DropAt(Vector3 position)
    {
        if (_items.IsEmpty()) return false;

        var stack = ItemStack.FindAt(position);
        if (stack.isFull)
            return false;

        stack.Push(_items.Pop());
        return true;
    }

    public bool MoveTo(ItemStack otherStack)
    {
        if (_items.IsEmpty() || otherStack.isFull) return false;

        var item = _items.Pop();
        otherStack.Push(item);
        return true;
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
            Destroy(item.gameObject);
    }
}