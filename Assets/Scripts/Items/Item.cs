using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour
{
    [SerializeField] float speed = 10;
    [SerializeField] float verticalSpeed = 2;
    [SerializeField] float gravity = -10;

    [SerializeField] AudioSource putDownSource;
    [SerializeField] AudioClip putDownClip;

    public ItemObject item;

    SpriteRenderer _spriteRenderer;

    bool _moving;
    Vector2 _destination;
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
        Vector2 position = transform.position;
        if (!_moving || position == _destination) return;

        var diff = _destination - position;
        var xClose = Mathf.Abs(diff.x) < 0.1f;
        var yClose = diff.y > -0.1f;

        if (xClose && yClose)
        {
            transform.position = _destination;
            _moving = false;
            // putDownSource.PlayOneShot(putDownClip);
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

    public void MoveTo(Vector2 destination)
    {
        _destination = destination;
        var horizontalDistance = _destination.x - transform.position.x;
        _velocity.x = horizontalDistance / Time.fixedDeltaTime / speed;
        _velocity.y = verticalSpeed;
        _moving = true;
    }

    public void StopMoving()
    {
        _moving = false;
    }

    void UpdateItemData()
    {
        if (!item) return;

        name = item.Name;
        GetComponent<SpriteRenderer>().sprite = item.sprite;
    }
}