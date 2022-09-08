using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class NodeEvent : MonoBehaviour
{
    [SerializeField] float _startDelayTime;
    [SerializeField] float _endDelayTime;

    public async UniTask Run(CancellationToken token)
    {
        if (_startDelayTime != 0)
            await UniTask.Delay(TimeSpan.FromSeconds(_startDelayTime), cancellationToken: token);

        await RunInternal(token);

        if (_endDelayTime != 0)
            await UniTask.Delay(TimeSpan.FromSeconds(_endDelayTime), cancellationToken: token);
    }

    public void RunAndForget()
    {
        Run(this.GetCancellationTokenOnDestroy()).Forget();
    }

    protected abstract UniTask RunInternal(CancellationToken token);
}