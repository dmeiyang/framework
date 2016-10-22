using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;

namespace System
{
    public static class JsonNet
    {
        public static T ToObjectByJsonNet<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string ToJsonByJsonNet(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static string GetJsonValue(this string jsonData, string key)
        {
            JObject obj = null;
            try
            {
                obj = JObject.Parse(jsonData);
            }
            catch
            {
                return null;
            }

            JToken token = null;
            obj.TryGetValue(key, out token);
            return token == null ? null : token.ToString();
        }

        public static string GetJsonValue(this JObject obj, string key)
        {
            JToken token = null;
            obj.TryGetValue(key, out token);

            return token == null ? null : token.ToString();
        }

        public static JArray ToJArray(this string jsonData)
        {
            try
            {
                return JArray.Parse(jsonData);
            }
            catch
            {
                return new JArray();
            }
        }

        public static JObject ToJObject(this string jsonData)
        {
            try
            {
                return JObject.Parse(jsonData);
            }
            catch
            {
                return new JObject();
            }
        }

        public static string ToEasyUIJson<T>(this T data, int count)
        {
            string body = "\"total\":{0},\"rows\":{1}";
            StringBuilder sb = new StringBuilder();
            string jsonResult = string.Empty;
            if (count == 0)
            {
                sb.Append("{");
                sb.Append(string.Format(body, count, "[]"));
                sb.Append("}");
                return sb.ToString();
            }
            else
            {
                try
                {
                    System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(data.GetType());
                    using (MemoryStream ms = new MemoryStream())
                    {
                        serializer.WriteObject(ms, data);
                        sb.Append("{");
                        sb.Append(string.Format(body, count, Encoding.UTF8.GetString(ms.ToArray())));
                        sb.Append("}");
                        return sb.ToString();
                    }
                }
                catch
                {
                    sb.Append("{");
                    sb.Append(string.Format(body, count, "[]"));
                    sb.Append("}");
                    return sb.ToString();
                }
            }
        }
    }
}
