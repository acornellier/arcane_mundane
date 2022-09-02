using System;
using Animancer;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AnimancerComponent))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 5;
    [SerializeField] int _maxStackSize = 3;

    [SerializeField] PlayerInteractors _interactors;
    [SerializeField] PlayerStack _itemStack;
    [SerializeField] Animations _animations;

    public bool canPickUpItem => _itemStack.count < _maxStackSize;

    PlayerInputActions.PlayerActions _actions;
    AnimancerComponent _animancer;
    Rigidbody2D _body;

    Vector2 _facingDirection = Vector2.up;

    void Awake()
    {
        _actions = new PlayerInputActions().Player;
        _animancer = GetComponent<AnimancerComponent>();
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
        UpdateAnimations();
    }

    public void PickUpItem(Item item)
    {
        if (!canPickUpItem) return;

        _itemStack.Push(item);
    }

    void OnInteract(InputAction.CallbackContext _)
    {
        _interactors.Interact();
    }

    void OnDrop(InputAction.CallbackContext _)
    {
        if (_itemStack.count <= 0) return;

        if (_interactors.currentInteractable != null &&
            _interactors.currentInteractable.TryGetComponent(out ItemStack groundStack))
        {
            _itemStack.MoveTo(groundStack);
            return;
        }

        _itemStack.DropAt(transform.position + (Vector3)_facingDirection);
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

        _facingDirection = moveInput.y == 0 ? moveInput : new Vector2(0, moveInput.y);

        if ((moveInput.x < 0 && transform.localScale.x > 0) ||
            (moveInput.x > 0 && transform.localScale.x < 0))
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);

        _interactors.UpdateInteractor(_facingDirection);
    }

    void UpdateAnimations()
    {
        _animancer.Play(_animations.idle.GetClip(_facingDirection));
    }

    [Serializable]
    class Animations
    {
        public DirectionalAnimationSet idle;
    }
}