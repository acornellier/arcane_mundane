using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class QuestPersistence : IDataPersistence
{
    [Inject(Id = InjectId.startQuest)] QuestNode _startQuest;

    public void Load(PersistentData allData)
    {
        var data = allData.questPersistence;

        if (data.completedQuestIds.IsEmpty())
        {
            _startQuest.gameObject.SetActive(true);
            return;
        }

        foreach (var questNode in Object.FindObjectsOfType<QuestNode>(true))
        {
            // if (data.completedQuestIds.Contains(questNode.id))
            // {
            // questNode.gameObject.SetActive(false);
            // }
            // else
            if (questNode.id == data.nextQuestId)
            {
                questNode.gameObject.SetActive(true);
                questNode.RunAndForget();
            }
        }
    }

    public void Save(PersistentData data)
    {
    }

    public class Data
    {
        public readonly List<int> completedQuestIds = new();
        public int nextQuestId;
    }
}