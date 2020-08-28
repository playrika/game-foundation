#if GF_NEWTONSOFT_JSON
using Newtonsoft.Json;

namespace Playrika.GameFoundation.Json
{
    public class NewtonsoftJsonConverter : IJsonConverter
    {
        public string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
#endif