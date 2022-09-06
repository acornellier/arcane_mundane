using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouse : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] PlayerStack _playerStack;
    [SerializeField] SpriteRenderer _stackMarker;

    [SerializeField] float _pickUpRange = 2;
    [SerializeField] float _dropRange = 3;

    Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void OnEnable()
    {
        _playerController.onLeftClick += HandleLeftClick;
        _playerController.onRightClick += HandleRightClick;
    }

    void OnDisable()
    {
        _playerController.onLeftClick -= HandleLeftClick;
        _playerController.onRightClick -= HandleRightClick;
    }

    void HandleLeftClick()
    {
        var mousePosition = RoundedMousePosition();
        if (Vector2.Distance(transform.position, mousePosition) < _pickUpRange)
            _playerStack.PickUpAt(mousePosition);
    }

    void HandleRightClick()
    {
        var mousePosition = RoundedMousePosition();
        if (Vector2.Distance(transform.position, mousePosition) < _dropRange)
            _playerStack.DropAt(mousePosition);
    }

    void Update()
    {
        _stackMarker.transform.position = RoundedMousePosition();
    }

    Vector2 RoundedMousePosition()
    {
        return Vector2Int.RoundToInt(
            _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue())
        );
    }
}