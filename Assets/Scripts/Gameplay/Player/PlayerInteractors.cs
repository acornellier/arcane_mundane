using UnityEngine;

public class PlayerInteractors : MonoBehaviour
{
    [SerializeField] PlayerInteractor up;
    [SerializeField] PlayerInteractor right;
    [SerializeField] PlayerInteractor down;

    PlayerInteractor _currentInteractor;

    public Vector3 currentInteractorPosition => _currentInteractor.transform.position;
    public Interactable currentInteractable => _currentInteractor.current;

    void Awake()
    {
        _currentInteractor = up;
    }

    void Start()
    {
        up.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        down.gameObject.SetActive(false);
    }

    public void Interact()
    {
        _currentInteractor.Interact();
    }

    public void UpdateInteractor(Vector2 facingDirection)
    {
        if (_currentInteractor)
            _currentInteractor.gameObject.SetActive(false);
        _currentInteractor = GetInteractor(facingDirection);
        _currentInteractor.gameObject.SetActive(true);
    }

    PlayerInteractor GetInteractor(Vector2 facingDirection)
    {
        if (facingDirection.x >= 0)
        {
            if (facingDirection.y >= 0)
                return facingDirection.x > facingDirection.y ? right : up;

            return facingDirection.x > -facingDirection.y ? right : down;
        }

        if (facingDirection.y >= 0)
            return facingDirection.x < -facingDirection.y ? right : up;

        return facingDirection.x < facingDirection.y ? right : down;
    }
}