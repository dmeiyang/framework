using System.Runtime.Serialization.Json;
using System.Text;

namespace System
{
    public static class DataContract
    {
        public static T ToObjectByDataContract<T>(this string json)
        {
            using (var ms = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            }
        }

        public static string ToJsonByDataContract(this object obj)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                new DataContractJsonSerializer(obj.GetType()).WriteObject(ms, obj);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
