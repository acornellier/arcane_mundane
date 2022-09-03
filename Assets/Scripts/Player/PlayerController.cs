using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions.PlayerActions _actions;

    public Action<bool> onReveal;

    void Awake()
    {
        _actions = new PlayerInputActions().Player;
    }

    void OnEnable()
    {
        _actions.Enable();
        _actions.Reveal.started += OnRevealStarted;
        _actions.Reveal.canceled += OnRevealCancelled;
    }

    void OnDisable()
    {
        _actions.Disable();
        _actions.Reveal.started -= OnRevealStarted;
        _actions.Reveal.canceled -= OnRevealCancelled;
    }

    void OnRevealStarted(InputAction.CallbackContext _)
    {
        onReveal?.Invoke(true);
    }

    void OnRevealCancelled(InputAction.CallbackContext _)
    {
        onReveal?.Invoke(false);
    }
}