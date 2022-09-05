using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Object = UnityEngine.Object;

public class PersistentDataManager
{
    static PersistentDataManager _instance;
    public static PersistentDataManager instance => _instance ??= new PersistentDataManager();

    PersistentData _persistentData;

    const string _key = "SavedData";

    public PersistentData data
    {
        get
        {
            if (_persistentData == null)
                LoadData();

            return _persistentData;
        }
        private set => _persistentData = value;
    }

    public void LoadObjects()
    {
        foreach (var dataPersistence in FindDataPersistences())
        {
            dataPersistence.Load(data);
        }
    }

    public void Save()
    {
        foreach (var dataPersistence in FindDataPersistences())
        {
            dataPersistence.Save();
        }

        var jsonString = JsonConvert.SerializeObject(data);
        PlayerPrefs.SetString(_key, jsonString);
        PlayerPrefs.Save();
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteKey(_key);
        LoadData();
    }

    public bool IsBoolSet(string key)
    {
        return data.bools.TryGetValue(key, out var value) && value;
    }

    public void SetBool(string key, bool value = true)
    {
        data.bools[key] = value;
    }

    void LoadData()
    {
        var jsonString = PlayerPrefs.GetString(_key);
        if (jsonString != "")
        {
            data = JsonConvert.DeserializeObject<PersistentData>(jsonString);
            return;
        }

        data = new PersistentData();
        Save();
    }

    static IEnumerable<IDataPersistence> FindDataPersistences()
    {
        return Object.FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
    }
}