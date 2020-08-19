using UnityEngine;

namespace Playrika.GameFoundation.Json
{
    public class UnityJsonConverter : IJsonConverter
    {
        public string SerializeObject(object obj)
        {
            return JsonUtility.ToJson(obj);
        }

        public T DeserializeObject<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
    }
}