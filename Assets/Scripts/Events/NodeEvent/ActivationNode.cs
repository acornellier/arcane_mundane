using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ActivationNode : NodeEvent
{
    [SerializeField] GameObject _gameObject;
    [SerializeField] bool _active = true;

    protected override async UniTask RunInternal(CancellationToken token)
    {
        if (_gameObject)
            _gameObject.SetActive(_active);

        await UniTask.Yield(token);
    }
}