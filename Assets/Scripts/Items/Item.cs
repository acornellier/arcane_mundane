using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour
{
    public ItemObject item;

    SpriteRenderer _spriteRenderer;

    Vector2 _localDestination;
    bool _moving;

    static readonly int _outlineThickness = Shader.PropertyToID("_OutlineThickness");

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.material = new Material(_spriteRenderer.material);
        UpdateItemData();
    }

    void OnValidate()
    {
        UpdateItemData();
    }

    void FixedUpdate()
    {
        Vector2 position = transform.localPosition;
        if (!_moving || position == _localDestination) return;

        if (Vector2.Distance(position, _localDestination) < 0.1f)
        {
            transform.localPosition = _localDestination;
            _moving = false;
            return;
        }

        var direction = _localDestination - position;
        transform.position += (Vector3)(10 * Time.fixedDeltaTime * direction);
    }

    public void Initialize(ItemObject itemObject)
    {
        item = itemObject;
        UpdateItemData();
    }

    public void Highlight()
    {
        _spriteRenderer.material.SetFloat(_outlineThickness, 1);
    }

    public void Unhighlight()
    {
        if (_spriteRenderer)
            _spriteRenderer.material.SetFloat(_outlineThickness, 0);
    }

    public void MoveToLocal(Vector2 localDestination)
    {
        _localDestination = localDestination;
        _moving = true;
    }

    void UpdateItemData()
    {
        if (!item) return;

        name = item.Name;
        GetComponent<SpriteRenderer>().sprite = item.sprite;
    }
}