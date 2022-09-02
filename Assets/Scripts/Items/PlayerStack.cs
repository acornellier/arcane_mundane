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

    public void DropAt(Vector3 position)
    {
        if (!_items.Any()) return;

        var stack = ItemStack.FindAt(position);
        if (stack.isFull)
            return;

        stack.Push(_items.Pop());
    }

    public void MoveTo(ItemStack otherStack)
    {
        if (!_items.Any() || otherStack.isFull) return;

        var item = _items.Pop();
        otherStack.Push(item);
    }
}