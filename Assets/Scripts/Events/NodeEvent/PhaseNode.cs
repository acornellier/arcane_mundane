using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class PhaseNode : NodeEvent
{
    [SerializeField] GamePhase _phase;

    [Inject] GameManager _gameManager;

    protected override async UniTask RunInternal(CancellationToken token)
    {
        _gameManager.SetPhase(_phase);
        await UniTask.Yield(token);
    }
}