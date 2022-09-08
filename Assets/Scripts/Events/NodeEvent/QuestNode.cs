using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class QuestNode : NodeSequence
{
    [SerializeField] bool _saveProgress = true;
    [SerializeField] QuestNode _nextQuest;

    [ReadOnly] public int id;

    [Inject] PersistentDataManager _persistentDataManager;

    protected void OnValidate()
    {
        if (id == 0)
            id = Random.Range(1, int.MaxValue);
    }

    protected override async UniTask RunInternal(CancellationToken token)
    {
        await base.RunInternal(token);

        if (_saveProgress)
            SaveProgress();

        if (_nextQuest)
        {
            _nextQuest.gameObject.SetActive(true);
            _nextQuest.RunAndForget();
        }
    }

    void SaveProgress()
    {
        var data = _persistentDataManager.data.questPersistence;
        data.completedQuestIds.Add(id);
        if (_nextQuest)
            data.nextQuestId = _nextQuest.id;

        _persistentDataManager.Save();
    }
}