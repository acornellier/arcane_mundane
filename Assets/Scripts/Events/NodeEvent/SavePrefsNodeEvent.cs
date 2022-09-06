using System.Collections;
using UnityEngine;
using Zenject;

public class SavePrefsNodeEvent : NodeEvent
{
    [SerializeField] string key = "default";
    [SerializeField] bool value = true;

    [Inject] PersistentDataManager _persistentDataManager;

    protected override IEnumerator CO_Run()
    {
        _persistentDataManager.data.bools[key] = value;
        _persistentDataManager.Save();
        yield break;
    }
}