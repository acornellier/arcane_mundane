using UnityEngine;

public class NodeEventInteractable : Interactable
{
    [SerializeField] NodeEvent _nodeEvent;

    bool _triggered;

    public override void Interact()
    {
        if (!_nodeEvent || _triggered) return;

        _triggered = true;
        _nodeEvent.gameObject.SetActive(true);
        _nodeEvent.RunAndForget();
    }

    public void SetNodeEvent(NodeEvent nodeEvent)
    {
        _nodeEvent = nodeEvent;
        _triggered = false;
    }
}