using System.Threading;
using Cysharp.Threading.Tasks;

public class NodeSequence : NodeEvent
{
    bool _debugSkip;

    protected override async UniTask RunInternal(CancellationToken token)
    {
        foreach (var nodeEvent in GetDirectChildrenNodes())
        {
            await nodeEvent.Run(token);
        }
    }
}