using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class PersistentDataManager : IInitializable, IDisposable
{
    const string _key = "SavedData";

    PersistentData _data;

    public PersistentData data => _data ??= ParseData();

    public void Initialize()
    {
        LoadObjects();
    }

    public void Dispose()
    {
        Save();
    }

    public void Save()
    {
        foreach (var dataPersistence in FindDataPersistences())
        {
            dataPersistence.Save(ref _data);
        }

        var jsonString = JsonConvert.SerializeObject(data);
        PlayerPrefs.SetString(_key, jsonString);
        PlayerPrefs.Save();
    }

    void LoadObjects()
    {
        foreach (var dataPersistence in FindDataPersistences())
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

    static IEnumerable<IDataPersistence> FindDataPersistences()
    {
        return Object.FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
    }
}