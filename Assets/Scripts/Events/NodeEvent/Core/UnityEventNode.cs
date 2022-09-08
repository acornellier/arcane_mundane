using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventNode : NodeEvent
{
    [SerializeField] UnityEvent unityEvent;

    protected override async UniTask RunInternal(CancellationToken token)
    {
        unityEvent.Invoke();
        await UniTask.Yield(token);
    }
}