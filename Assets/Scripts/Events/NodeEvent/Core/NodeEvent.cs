using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class NodeEvent : MonoBehaviour
{
    [SerializeField] float _startDelay;
    [SerializeField] float _endDelay;

    public async UniTask Run(CancellationToken token)
    {
        if (_startDelay != 0)
            await UniTask.Delay(TimeSpan.FromSeconds(_startDelay), cancellationToken: token);

        await RunInternal(token);

        if (_endDelay != 0)
            await UniTask.Delay(TimeSpan.FromSeconds(_endDelay), cancellationToken: token);
    }

    public void RunAndForget()
    {
        Run(this.GetCancellationTokenOnDestroy()).Forget();
    }

    protected abstract UniTask RunInternal(CancellationToken token);

    protected IEnumerable<NodeEvent> GetDirectChildrenNodes()
    {
        return gameObject.GetComponentsInDirectChildren<NodeEvent>()
            .Where(nodeEvent => nodeEvent.gameObject.activeInHierarchy);
    }
}