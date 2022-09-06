public interface IDataPersistence
{
    void Load(PersistentData data);
    void Save(ref PersistentData data);
}