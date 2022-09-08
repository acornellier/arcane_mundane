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
    [SerializeField] PlayerController _controller;
    [SerializeField] PlayerStack _itemStack;
    [SerializeField] Animations _animations;

    public Item topOfStack => _itemStack.Peek();

    AnimancerComponent _animancer;
    Rigidbody2D _body;

    Vector2Int _facingDirection = Vector2Int.down;

    void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
        _body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        UpdateMovement();
        UpdateDirection();
        UpdateAnimations();
    }

    public void Load(PersistentData data)
    {
        if (data.player == null) return;

        transform.position = data.player.position;
        SetFacingDirection(data.player.facingDirection);
    }

    public void Save(PersistentData data)
    {
        data.player = new Data
        {
            position = transform.position,
            facingDirection = _facingDirection,
        };
    }

    public void Footstep()
    {
        _audio.Footstep();
    }

    public void DestroyTop()
    {
        _itemStack.DestroyTop();
    }

    void UpdateMovement()
    {
        var moveInput = _controller.actions.Move.ReadValue<Vector2>();
        var runInput = _controller.actions.Run.IsPressed();

        var adjustedSpeed = _speed - _stackSpeedPercentReduction * _speed * _itemStack.count;
        if (runInput)
            adjustedSpeed *= _runMultiplier;

        var movement = adjustedSpeed * Time.fixedDeltaTime * moveInput;

        _body.MovePosition((Vector2)transform.position + movement);
    }

    void UpdateDirection()
    {
        var moveInput = _controller.actions.Move.ReadValue<Vector2>();
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
    }

    void UpdateAnimations()
    {
        var directionalAnimationSet = GetDirectionalAnimationSet();
        var state = _animancer.Play(directionalAnimationSet.GetClip(_facingDirection));
        state.Speed = 1 - _itemStack.count * _stackSpeedPercentReduction;
    }

    DirectionalAnimationSet GetDirectionalAnimationSet()
    {
        var moveInput = _controller.actions.Move.ReadValue<Vector2>();
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
        public Vector3 position;
        public Vector2Int facingDirection;
    }
}