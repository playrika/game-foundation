using System;
using System.Globalization;
using System.IO;
using UnityEngine;
using Playrika.GameFoundation.Helpers;
using Playrika.GameFoundation.Json;

namespace Playrika.GameFoundation.DataManagement
{
    public class JsonDiskDataStorage : IDataStorage
    {
        private readonly IJsonConverter _jsonConverter;

        private readonly string _path;

        public JsonDiskDataStorage(IJsonConverter jsonConverter, string path)
        {
            _jsonConverter = jsonConverter;
            _path = path;

            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
        }

        public T Get<T>(string key, T defaultValue)
        {
            var path = GetPath(key);
            if (!File.Exists(path))
                return defaultValue;

            var json = File.ReadAllText(path);
            if (string.IsNullOrEmpty(json))
                return defaultValue;

            var type = typeof(T);
            if (TypeHelper.IsPrimitiveOrString(type))
                return (T) Convert.ChangeType(json, type);

            return _jsonConverter.DeserializeObject<T>(json);
        }

        public void Set<T>(string key, T value)
        {
            var type = typeof(T);

            var json = TypeHelper.IsPrimitiveOrString(type)
                ? string.Format(CultureInfo.InvariantCulture, "{0}", value)
                : _jsonConverter.SerializeObject(value);

            try
            {
                File.WriteAllText(GetPath(key), json);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public void Delete(string key)
        {
            var path = GetPath(key);

            if (File.Exists(path))
                File.Delete(path);
        }

        public void DeleteAll()
        {
            var files = Directory.GetFiles(_path);

            foreach (var file in files)
                File.Delete(file);
        }

        private string GetPath(string key)
        {
            return Path.Combine(_path, key + ".json");
        }
    }
}