using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineNode : NodeEvent
{
    [SerializeField] PlayableDirector _timeline;

    protected override async UniTask RunInternal(CancellationToken token)
    {
        _timeline.Play();
        await UniTask.WaitUntil(
            () => _timeline.time >= _timeline.playableAsset.duration - 0.1f,
            cancellationToken: token
        );
    }
}