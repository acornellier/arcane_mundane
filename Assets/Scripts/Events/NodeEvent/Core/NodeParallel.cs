using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class NodeParallel : NodeEvent
{
    [SerializeField] List<NodeEvent> _nodeEvents;
    [SerializeField] float _timeBetweenStarts;

    void Awake()
    {
        _nodeEvents = gameObject.GetComponentsInDirectChildren<NodeEvent>()
            .Where(nodeEvent => nodeEvent.gameObject.activeInHierarchy)
            .ToList();
    }

    protected override async UniTask RunInternal(CancellationToken token)
    {
        var tasks = new List<UniTask>();
        foreach (var nodeEvent in _nodeEvents)
        {
            tasks.Add(nodeEvent.Run(token));
            if (_timeBetweenStarts > 0)
                await UniTask.Delay(
                    TimeSpan.FromSeconds(_timeBetweenStarts),
                    cancellationToken: token
                );
        }

        await UniTask.WhenAll(tasks);
    }
}