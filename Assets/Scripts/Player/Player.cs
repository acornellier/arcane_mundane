using System;
using System.Collections;
using Animancer;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AnimancerComponent))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] float _speed = 7;
    [SerializeField] float _stackSpeedPercentReduction = 0.1f;
    [SerializeField] int _maxStackSize = 3;

    [SerializeField] PlayerAudio _audio;
    [SerializeField] PlayerInteractors _interactors;
    [SerializeField] PlayerStack _itemStack;
    [SerializeField] SpriteRenderer _interactMarker;
    [SerializeField] Animations _animations;

    public bool canPickUpItem => _itemStack.count < _maxStackSize;
    public Item topOfStack => _itemStack.Peek();

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
        _actions.HardDrop.performed += OnHardDrop;
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
        UpdateGroundMarker();
        UpdateAnimations();
    }

    public void Footstep()
    {
        _audio.Footstep();
    }

    public void PickUpItem(Item item)
    {
        if (!canPickUpItem) return;

        _itemStack.Push(item);
    }

    public void DestroyTop()
    {
        _itemStack.DestroyTop();
    }

    void OnInteract(InputAction.CallbackContext _)
    {
        _interactors.Interact();
    }

    void OnDrop(InputAction.CallbackContext _)
    {
        if (_itemStack.count <= 0) return;

        Drop();
    }

    bool Drop()
    {
        if (_interactors.currentInteractable != null &&
            _interactors.currentInteractable.TryGetComponent(out ItemStack groundStack))
            return _itemStack.MoveTo(groundStack);

        return false;
        // return _itemStack.DropAt(transform.position + (Vector3)_facingDirection);
    }

    void OnHardDrop(InputAction.CallbackContext _)
    {
        StartCoroutine(CO_HardDrop());
    }

    IEnumerator CO_HardDrop()
    {
        while (Drop())
        {
            yield return new WaitForSeconds(0.1f);
        }
    }

    void UpdateMovement()
    {
        var moveInput = _actions.Move.ReadValue<Vector2>();
        var adjustedSpeed = _speed - _stackSpeedPercentReduction * _speed * _itemStack.count;
        var newPosition =
            (Vector2)transform.position + adjustedSpeed * Time.fixedDeltaTime * moveInput;

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
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            _itemStack.transform.localScale = transform.localScale;
        }

        _interactors.UpdateInteractor(_facingDirection);
    }

    void UpdateGroundMarker()
    {
        if (_interactors.currentInteractable == null)
        {
            _interactMarker.enabled = false;
            return;
        }

        _interactMarker.enabled = true;
        _interactMarker.transform.position = _interactors.currentInteractable.transform.position;
    }

    void UpdateAnimations()
    {
        var directionalAnimationSet = GetDirectionalAnimationSet();
        var state = _animancer.Play(directionalAnimationSet.GetClip(_facingDirection));
        state.Speed = 1 - _itemStack.count * _stackSpeedPercentReduction;
    }

    DirectionalAnimationSet GetDirectionalAnimationSet()
    {
        var moveInput = _actions.Move.ReadValue<Vector2>();
        return _itemStack.Any()
            ? moveInput == default
                ? _animations.carryIdle
                : _animations.carryWalk
            : moveInput == default
                ? _animations.idle
                : _animations.walk;
    }

    [Serializable]
    class Animations
    {
        public DirectionalAnimationSet idle;
        public DirectionalAnimationSet walk;
        public DirectionalAnimationSet carryIdle;
        public DirectionalAnimationSet carryWalk;
    }
}