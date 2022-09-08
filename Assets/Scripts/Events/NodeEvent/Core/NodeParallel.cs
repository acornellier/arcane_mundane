using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class NodeParallel : NodeEvent
{
    [SerializeField] float _timeBetweenStarts;

    protected override async UniTask RunInternal(CancellationToken token)
    {
        var tasks = new List<UniTask>();
        foreach (var nodeEvent in GetDirectChildrenNodes())
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