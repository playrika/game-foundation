using System;
using System.Globalization;
using UnityEngine;
using Playrika.GameFoundation.Helpers;
using Playrika.GameFoundation.Json;

namespace Playrika.GameFoundation.DataManagement
{
    public class PlayerPrefsDataStorage : IDataStorage
    {
        private readonly IJsonConverter _jsonConverter;

        public PlayerPrefsDataStorage(IJsonConverter jsonConverter)
        {
            _jsonConverter = jsonConverter;
        }

        public T Get<T>(string key, T defaultValue)
        {
            var json = PlayerPrefs.GetString(key, null);

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

            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }

        public void Delete(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}