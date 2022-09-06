using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

public class PersistentDataManager : IInitializable
{
    [Inject] IEnumerable<IDataPersistence> _dataPersistences;

    PersistentData _data;

    public PersistentData data => _data ??= ParseData();

    const string _key = "SavedData";

    public void Initialize()
    {
        LoadObjects();
    }

    public void Save()
    {
        foreach (var dataPersistence in _dataPersistences)
        {
            dataPersistence.Save(_data);
        }

        var jsonString = JsonConvert.SerializeObject(data);
        PlayerPrefs.SetString(_key, jsonString);
        PlayerPrefs.Save();
    }

    public static void Reset()
    {
        PlayerPrefs.SetString(_key, "");
        PlayerPrefs.Save();
    }

    void LoadObjects()
    {
        foreach (var dataPersistence in _dataPersistences)
        {
            dataPersistence.Load(data);
        }
    }

    public bool IsBoolSet(string key)
    {
        return data.bools.TryGetValue(key, out var value) && value;
    }

    public void SetBool(string key, bool value = true)
    {
        data.bools[key] = value;
    }

    static PersistentData ParseData()
    {
        var jsonString = PlayerPrefs.GetString(_key);
        return jsonString == ""
            ? new PersistentData()
            : JsonConvert.DeserializeObject<PersistentData>(jsonString);
    }
}