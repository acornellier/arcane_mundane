using System.Threading;
using Cysharp.Threading.Tasks;
using Zenject;

public class SaveNode : NodeEvent
{
    [Inject] PersistentDataManager _persistentDataManager;

    protected override async UniTask RunInternal(CancellationToken token)
    {
        _persistentDataManager.Save();
        await UniTask.Yield(token);
    }
}