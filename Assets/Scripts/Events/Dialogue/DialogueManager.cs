using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] DialogueImage topImage;

    public bool isActive;
    public Action onDialogueStart;
    public Action onDialogueEnd;

    PlayerInputActions.PlayerActions _actions;

    DialogueImage _activeDialogueImage;
    Queue<Dialogue> _dialogues;
    Action _callback;

    void Awake()
    {
        _actions = new PlayerInputActions().Player;
        _actions.LeftClick.performed += OnNextInput;
        _actions.RightClick.performed += OnSkipinput;
        _actions.Interact.performed += OnNextInput;
    }

    void Start()
    {
        topImage.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        StopDialogue();
    }

    public void StartDialogue(IEnumerable<Dialogue> dialogues)
    {
        if (isActive)
            throw new Exception("Dialogue Manager is already active");

        isActive = true;
        onDialogueStart?.Invoke();
        _actions.Enable();

        _dialogues = new Queue<Dialogue>(dialogues);
        TypeNextLine();
    }

    void StopDialogue()
    {
        _actions.Disable();
        if (_activeDialogueImage)
        {
            _activeDialogueImage.StopCoroutine();
            _activeDialogueImage.gameObject.SetActive(false);
            _activeDialogueImage = null;
        }

        isActive = false;
        onDialogueEnd?.Invoke();
    }

    void OnNextInput(InputAction.CallbackContext ctx)
    {
        if (_activeDialogueImage && !_activeDialogueImage.IsDone())
        {
            _activeDialogueImage.SkipToEndOfLine();
            return;
        }

        TypeNextLine();
    }

    void OnSkipinput(InputAction.CallbackContext obj)
    {
        StopDialogue();
    }

    void TypeNextLine()
    {
        if (_dialogues.Count <= 0)
        {
            StopDialogue();
            return;
        }

        if (_activeDialogueImage)
            _activeDialogueImage.gameObject.SetActive(false);

        var nextDialogue = _dialogues.Dequeue();
        _activeDialogueImage = topImage;
        _activeDialogueImage.gameObject.SetActive(true);
        _activeDialogueImage.TypeNextLine(nextDialogue);
    }
}