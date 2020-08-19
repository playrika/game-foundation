namespace Playrika.GameFoundation.DataManagement
{
    public interface IDataStorage
    {
        T Get<T>(string key, T defaultValue);

        void Set<T>(string key, T value);

        void Delete(string key);

        void DeleteAll();
    }
}