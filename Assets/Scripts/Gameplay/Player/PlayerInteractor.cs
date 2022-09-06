using System.Collections.Generic;
using MoreLinq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteractor : MonoBehaviour
{
    readonly HashSet<Interactable> _list = new();

    public Interactable current { get; private set; }

    Vector3 _lastPosition;
    bool _changed;

    void Update()
    {
        if (!_changed && transform.position == _lastPosition)
            return;

        _changed = false;
        _lastPosition = transform.position;

        var nearest = NearestInteractable();
        if (nearest == current) return;

        current = nearest;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var interactable = col.GetComponent<Interactable>();
        if (interactable == null) return;

        if (_list.Add(interactable))
            _changed = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        var interactable = col.GetComponent<Interactable>();

        if (_list.Remove(interactable))
            _changed = true;
    }

    public void Interact()
    {
        if (current)
            current.Interact();
    }

    Interactable NearestInteractable()
    {
        if (_list.IsEmpty()) return null;

        return _list.MinBy(
            interactable => Vector2.Distance(
                transform.position,
                interactable.transform.position
            )
        ).First();
    }
}