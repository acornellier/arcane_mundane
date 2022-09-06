using System;
using Animancer;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AnimancerComponent))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IDataPersistence
{
    [SerializeField] float _speed = 7;
    [SerializeField] float _runMultiplier = 1.5f;
    [SerializeField] float _stackSpeedPercentReduction = 0.1f;

    [SerializeField] PlayerAudio _audio;
    [SerializeField] PlayerInteractors _interactors;
    [SerializeField] PlayerStack _itemStack;
    [SerializeField] Animations _animations;

    public Item topOfStack => _itemStack.Peek();

    PlayerInputActions.PlayerActions _actions;
    AnimancerComponent _animancer;
    Rigidbody2D _body;

    Vector2Int _facingDirection = Vector2Int.up;

    void Awake()
    {
        _actions = new PlayerInputActions().Player;
        _actions.Interact.performed += OnInteract;
        _animancer = GetComponent<AnimancerComponent>();
        _body = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
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

    public void Load(PersistentData data)
    {
        if (data.player.position == null) return;

        transform.position = PersistentData.ArrayToVector3(data.player.position);
        SetFacingDirection(PersistentData.ArrayToVector2Int(data.player.facingDirection));
    }

    public void Save(PersistentData data)
    {
        data.player.position = PersistentData.Vector3ToArr(transform.position);
        data.player.facingDirection = PersistentData.Vector2IntToArr(_facingDirection);
    }

    public void Footstep()
    {
        _audio.Footstep();
    }

    public void DestroyTop()
    {
        _itemStack.DestroyTop();
    }

    void OnInteract(InputAction.CallbackContext _)
    {
        _interactors.Interact();
    }

    void UpdateMovement()
    {
        var moveInput = _actions.Move.ReadValue<Vector2>();
        var runInput = _actions.Run.IsPressed();

        var adjustedSpeed = _speed - _stackSpeedPercentReduction * _speed * _itemStack.count;
        if (runInput)
            adjustedSpeed *= _runMultiplier;

        var movement = adjustedSpeed * Time.fixedDeltaTime * moveInput;

        _body.MovePosition((Vector2)transform.position + movement);
    }

    void UpdateDirection()
    {
        var moveInput = _actions.Move.ReadValue<Vector2>();
        if (moveInput == default || moveInput == _facingDirection)
            return;

        SetFacingDirection(
            moveInput.y == 0
                ? Vector2Int.RoundToInt(moveInput)
                : new Vector2Int(0, Mathf.RoundToInt(moveInput.y))
        );
    }

    void SetFacingDirection(Vector2Int facingDirection)
    {
        _facingDirection = facingDirection;

        if ((_facingDirection.x < 0 && transform.localScale.x > 0) ||
            (_facingDirection.x > 0 && transform.localScale.x < 0))
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            _itemStack.transform.localScale = transform.localScale;
        }

        _interactors.UpdateInteractor(_facingDirection);
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

    public class Data
    {
        public float[] position;
        public int[] facingDirection;
    }
}