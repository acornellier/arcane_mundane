using System.Threading;
using Cysharp.Threading.Tasks;

public class EmptyNode : NodeEvent
{
    protected override async UniTask RunInternal(CancellationToken token)
    {
        await UniTask.Yield(token);
    }
}