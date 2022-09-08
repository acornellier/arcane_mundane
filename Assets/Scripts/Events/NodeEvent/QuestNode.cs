using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

public class QuestNode : NodeEvent
{
    [SerializeField] bool _runOnStart;
    [SerializeField] bool _saveOnEnd = true;
    [SerializeField] NodeEvent _onEnable;
    [SerializeField] NodeEvent _mainNode;
    [SerializeField] QuestNode _nextQuest;

    [ReadOnly] public int id;

    [Inject] PersistentDataManager _persistentDataManager;

    void OnEnable()
    {
        if (_onEnable)
            _onEnable.RunAndForget();
    }

    void Start()
    {
        if (_runOnStart)
            RunAndForget();
    }

    protected void OnValidate()
    {
        if (id == 0)
            id = Random.Range(1, int.MaxValue);

        if (_mainNode == this)
            _mainNode = null;
    }

    protected override async UniTask RunInternal(CancellationToken token)
    {
        await _mainNode.Run(token);

        if (_saveOnEnd)
            SaveProgress();

        if (_nextQuest)
            _nextQuest.gameObject.SetActive(true);
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