using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 5;
    [SerializeField] int _maxStackSize = 3;
    [SerializeField] Item itemPrefab;

    [SerializeField] PlayerInteractors _interactors;

    PlayerInputActions.PlayerActions _actions;
    Rigidbody2D _body;

    readonly Stack<ItemObject> _itemStack = new();
    Vector2 _facingDirection = Vector2.up;

    void Awake()
    {
        _actions = new PlayerInputActions().Player;
        _body = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        _actions.Interact.performed += OnInteract;
        _actions.Drop.performed += OnDrop;
        _actions.Enable();
    }

    void OnDisable()
    {
        _actions.Disable();
    }

    void Start()
    {
        _interactors.UpdateInteractor(_facingDirection);
    }

    void FixedUpdate()
    {
        UpdateMovement();
        UpdateDirection();
    }

    public bool PickUpItem(ItemObject item)
    {
        if (_itemStack.Count() >= _maxStackSize) return false;

        _itemStack.Push(item);
        return true;
    }

    void OnInteract(InputAction.CallbackContext _)
    {
        _interactors.Interact();
    }

    void OnDrop(InputAction.CallbackContext _)
    {
        if (!_itemStack.Any()) return;

        var dropPosition = transform.position + (Vector3)_facingDirection;
        var obj = Instantiate(itemPrefab, dropPosition, Quaternion.identity);
        obj.Initialize(_itemStack.Pop());
    }

    void UpdateMovement()
    {
        var moveInput = _actions.Move.ReadValue<Vector2>();
        var newPosition =
            (Vector2)transform.position + _speed * Time.fixedDeltaTime * moveInput;

        _body.MovePosition(newPosition);
    }

    void UpdateDirection()
    {
        var moveInput = _actions.Move.ReadValue<Vector2>();
        if (moveInput == default || moveInput == _facingDirection)
            return;

        _facingDirection = moveInput;

        if ((moveInput.x < 0 && transform.localScale.x > 0) ||
            (moveInput.x > 0 && transform.localScale.x < 0))
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);

        _interactors.UpdateInteractor(_facingDirection);
    }
}