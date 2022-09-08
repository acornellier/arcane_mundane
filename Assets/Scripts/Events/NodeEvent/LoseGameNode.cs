using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class LoseGameNode : NodeEvent
{
    [Inject] MainTimer _mainTimer;

    protected override async UniTask RunInternal(CancellationToken token)
    {
        _mainTimer.LoseGame();
        await UniTask.Yield(token);
    }
}