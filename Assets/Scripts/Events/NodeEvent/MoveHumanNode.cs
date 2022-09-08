using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class MoveHumanNode : NodeEvent
{
    [SerializeField] Human _human;
    [SerializeField] bool _entering;

    protected override async UniTask RunInternal(CancellationToken token)
    {
        await _human.Move(_entering, token);
    }
}