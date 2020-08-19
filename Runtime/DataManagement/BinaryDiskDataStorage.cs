using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Playrika.GameFoundation.DataManagement
{
    public class BinaryDiskDataStorage : IDataStorage
    {
        private readonly string _path;

        public BinaryDiskDataStorage(string path)
        {
            _path = path;

            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
        }

        public T Get<T>(string key, T defaultValue)
        {
            var path = Path.Combine(_path, key);
            if (!File.Exists(path))
                return defaultValue;

            var bytes = File.ReadAllBytes(path);
            var type = typeof(T);

            if (bytes.GetType() == type)
            {
                object obj = bytes;
                return (T) obj;
            }

            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    var binaryFormatter = new BinaryFormatter();
                    memoryStream.Write(bytes, 0, bytes.Length);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    return (T) binaryFormatter.Deserialize(memoryStream);
                }
                catch (SerializationException e)
                {
                    Debug.LogError(e);
                }
            }

            return defaultValue;
        }

        public void Set<T>(string key, T value)
        {
            var path = Path.Combine(_path, key);

            using (var fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                var formatter = new BinaryFormatter();

                try
                {
                    formatter.Serialize(fs, value);
                }
                catch (SerializationException e)
                {
                    Debug.LogError(e);
                }
            }
        }

        public void Delete(string key)
        {
            var path = Path.Combine(_path, key);

            if (File.Exists(path))
                File.Delete(path);
        }

        public void DeleteAll()
        {
            var files = Directory.GetFiles(_path);

            foreach (var file in files)
                File.Delete(file);
        }
    }
}