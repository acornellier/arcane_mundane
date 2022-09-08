using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [Inject] DialogueManager _dialogueManager;

    public PlayerInputActions.PlayerActions actions;

    public Action<bool> onReveal;
    public Action onLeftClick;
    public Action onRightClick;

    void Awake()
    {
        actions = new PlayerInputActions().Player;
        actions.LeftClick.performed += HandleLeftClick;
        actions.RightClick.performed += HandleRightClick;
        actions.Reveal.started += OnRevealStarted;
        actions.Reveal.canceled += OnRevealCancelled;
    }

    void OnEnable()
    {
        EnableControls();
        _dialogueManager.onDialogueStart += DisableControls;
        _dialogueManager.onDialogueEnd += EnableControls;
    }

    void OnDisable()
    {
        DisableControls();
        _dialogueManager.onDialogueStart -= DisableControls;
        _dialogueManager.onDialogueEnd -= EnableControls;
    }

    void EnableControls()
    {
        actions.Enable();
    }

    void DisableControls()
    {
        actions.Disable();
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