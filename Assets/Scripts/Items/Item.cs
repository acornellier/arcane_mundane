using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour
{
    [SerializeField] AudioSource putDownSource;
    [SerializeField] AudioClip putDownClip;

    public ItemObject itemObject;

    const float _speed = 10;
    const float _verticalSpeed = 3;
    const float _gravity = -50;

    bool _moving;
    Vector2 _destination;
    Vector2 _velocity;

    void Awake()
    {
        UpdateItemData();
    }

    void OnValidate()
    {
        UpdateItemData();
    }

    void FixedUpdate()
    {
        if (!_moving) return;

        var diff = _destination - (Vector2)transform.position;
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

        _velocity.y += _gravity * Time.fixedDeltaTime;
        transform.position += Time.fixedDeltaTime * (Vector3)_velocity;
    }

    public void Initialize(ItemObject newItemObject)
    {
        itemObject = newItemObject;
        UpdateItemData();
    }

    public void MoveTo(Vector2 destination)
    {
        _destination = destination;
        var horizontalDistance = _destination.x - transform.position.x;
        _velocity.x = horizontalDistance / Time.fixedDeltaTime / _speed;
        _velocity.y = _verticalSpeed;
        _moving = true;
    }

    public void StopMoving()
    {
        _moving = false;
    }

    void UpdateItemData()
    {
        if (!itemObject) return;

        name = itemObject.Name;
        GetComponent<SpriteRenderer>().sprite = itemObject.sprite;
    }
}