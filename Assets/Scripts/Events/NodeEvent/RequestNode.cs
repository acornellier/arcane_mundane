using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class RequestNode : NodeEvent
{
    [SerializeField] float _duration = 30;
    [SerializeField] int _numberOfItems = 1;
    [SerializeField] Requester _requester;

    [Inject] MainTimer _mainTimer;

    protected override async UniTask RunInternal(CancellationToken token)
    {
        await _mainTimer.RunTimer(token, _requester, _duration, _numberOfItems);
    }
}