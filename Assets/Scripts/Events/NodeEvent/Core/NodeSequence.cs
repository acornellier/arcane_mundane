using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class NodeSequence : NodeEvent
{
    [SerializeField] List<NodeEvent> _nodeEvents;
    [SerializeField] bool _playOnStart;

    bool _debugSkip;

    void Awake()
    {
        _nodeEvents = gameObject.GetComponentsInDirectChildren<NodeEvent>()
            .Where(nodeEvent => nodeEvent.gameObject.activeInHierarchy)
            .ToList();
    }

    void Start()
    {
        if (_playOnStart)
            RunAndForget();
    }

    protected override async UniTask RunInternal(CancellationToken token)
    {
        foreach (var nodeEvent in _nodeEvents)
        {
            await nodeEvent.Run(token);
        }
    }
}