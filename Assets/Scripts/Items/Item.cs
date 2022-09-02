using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour
{
    [SerializeField] float speed = 10;
    [SerializeField] float verticalSpeed = 2;
    [SerializeField] float gravity = -10;

    public ItemObject item;

    SpriteRenderer _spriteRenderer;

    bool _moving;
    Vector2 _localDestination;
    Vector2 _velocity;

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

        var diff = _localDestination - position;
        var xClose = Mathf.Abs(diff.x) < 0.1f;
        var yClose = diff.y > -0.1f;

        if (xClose && yClose)
        {
            transform.localPosition = _localDestination;
            _moving = false;
            return;
        }

        if (xClose)
            _velocity.x = 0;

        if (yClose)
            _velocity.y = 0;

        _velocity.y += gravity * Time.fixedDeltaTime;
        transform.position += Time.fixedDeltaTime * (Vector3)_velocity;
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
        var horizontalDistance = _localDestination.x - transform.localPosition.x;
        _velocity.x = horizontalDistance / Time.fixedDeltaTime / speed;
        _velocity.y = verticalSpeed;
        _moving = true;
    }

    void UpdateItemData()
    {
        if (!item) return;

        name = item.Name;
        GetComponent<SpriteRenderer>().sprite = item.sprite;
    }
}