using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteractor : MonoBehaviour
{
    IInteractable _interactable;

    public bool hasInteractable => _interactable != null;

    void OnDisable()
    {
        _interactable?.Unhighlight();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (_interactable != null) return;

        var interactable = col.GetComponent<IInteractable>();
        if (interactable != null)
        {
            _interactable = interactable;
            _interactable.Highlight();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        var interactable = col.GetComponent<IInteractable>();
        if (interactable == _interactable)
        {
            _interactable?.Unhighlight();
            _interactable = null;
        }
    }

    public void Interact()
    {
        _interactable?.Interact();
    }
}