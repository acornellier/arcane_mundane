using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions.PlayerActions _actions;

    public Action<bool> onReveal;
    public Action onLeftClick;
    public Action onRightClick;

    void Awake()
    {
        _actions = new PlayerInputActions().Player;
        _actions.LeftClick.performed += HandleLeftClick;
        _actions.RightClick.performed += HandleRightClick;
        _actions.Reveal.started += OnRevealStarted;
        _actions.Reveal.canceled += OnRevealCancelled;
    }

    void OnEnable()
    {
        _actions.Enable();
    }

    void OnDisable()
    {
        _actions.Disable();
    }

    void OnRevealStarted(InputAction.CallbackContext _)
    {
        onReveal?.Invoke(true);
    }

    void OnRevealCancelled(InputAction.CallbackContext _)
    {
        onReveal?.Invoke(false);
    }

    void HandleLeftClick(InputAction.CallbackContext obj)
    {
        onLeftClick?.Invoke();
    }

    void HandleRightClick(InputAction.CallbackContext obj)
    {
        onRightClick?.Invoke();
    }
}