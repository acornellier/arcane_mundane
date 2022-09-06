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

    void Start()
    {
        foreach (var item in GetComponentsInChildren<Item>())
        {
            if (isFull)
            {
                Destroy(item);
                continue;
            }

            if (!items.Contains(item))
                Push(item);
        }
    }

    public void Push(Item item)
    {
        if (isFull) return;

        item.transform.parent = transform;
        item.MoveTo((Vector2)transform.position + items.Count * _spacing * Vector2.up);
        item.GetComponent<SpriteRenderer>().sortingOrder = items.Count;

        items.Push(item);
    }

    public Item Pop()
    {
        var item = items.Pop();
        item.transform.parent = null;

        if (items.IsEmpty())
            Destroy(gameObject);

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
        public float[] position;
        public string[] itemObjectNames;
    }
}