using System.Collections.Generic;
using UnityEngine;

public class ItemStack : MonoBehaviour
{
    public bool isFull => items.Count >= _maxSize;
    public Stack<Item> items { get; } = new();

    const float _spacing = 0.25f;
    const float _underneathAlpha = 0.5f;
    const int _maxSize = 3;

    PlayerController _playerController;

    void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    void OnEnable()
    {
        _playerController.onReveal += SetUnderneathAlpha;
    }

    void OnDisable()
    {
        _playerController.onReveal -= SetUnderneathAlpha;
    }

    public void Push(Item item, bool animate = true)
    {
        if (isFull) return;

        item.transform.parent = transform;
        item.GetComponent<SpriteRenderer>().sortingOrder = items.Count;

        var destination = (Vector2)transform.position + items.Count * _spacing * Vector2.up;
        if (animate)
            item.ThrowAt(destination);
        else
            item.transform.position = destination;

        items.Push(item);
    }

    public Item Pop()
    {
        var item = items.Pop();
        item.transform.parent = null;

        if (items.IsEmpty())
            Utilities.DestroyGameObject(gameObject);

        return item;
    }

    void SetUnderneathAlpha(bool reveal)
    {
        var first = true;
        foreach (var item in items)
        {
            var spriteRenderer = item.GetComponent<SpriteRenderer>();
            var color = spriteRenderer.color;
            color.a = first || !reveal ? 1 : _underneathAlpha;
            spriteRenderer.color = color;
            first = false;
        }
    }

    public class Data
    {
        public Vector3 position;
        public string[] itemObjectNames;
    }
}