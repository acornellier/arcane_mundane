using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteractor : MonoBehaviour
{
    readonly List<Interactable> _list = new();

    public Interactable current => _list.FirstOrDefault();

    void OnDisable()
    {
        _list.FirstOrDefault()?.Unhighlight();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var interactable = col.GetComponent<Interactable>();
        if (interactable == null) return;

        _list.Add(interactable);
        if (_list.Count == 1)
            interactable.Highlight();
    }

    void OnTriggerExit2D(Collider2D col)
    {
        var interactable = col.GetComponent<Interactable>();

        var index = _list.IndexOf(interactable);
        if (index == -1) return;

        if (index == 0)
        {
            _list[index].Unhighlight();
            if (_list.Count > 1)
                _list[1].Highlight();
        }

        _list.RemoveAt(index);
    }

    public void Interact()
    {
        _list.FirstOrDefault()?.Interact();
    }
}