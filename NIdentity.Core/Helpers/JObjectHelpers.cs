using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NIdentity.Core.Helpers
{
    public static class JObjectHelpers
    {
        /// <summary>
        /// (Fluent) Set the property to be value.
        /// </summary>
        /// <param name="Json"></param>
        /// <param name="Property"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static JObject Set(this JObject Json, string Property, JToken Value)
        {
            Json[Property] = Value;
            return Json;
        }

        /// <summary>
        /// (Fluent) Set the property to be value.
        /// </summary>
        /// <param name="Json"></param>
        /// <param name="Property"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static JObject Set(this JObject Json, string Property, string Value)
        {
            Json[Property] = Value;
            return Json;
        }

        /// <summary>
        /// (Fluent) Test whether the property defined or not.
        /// </summary>
        /// <param name="Json"></param>
        /// <param name="Property"></param>
        /// <returns></returns>
        public static bool Has(this JObject Json, string Property)
        {
            try { return Json.Property(Property) != null; }
            catch { return false; }
        }

        /// <summary>
        /// Get the value.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="Json"></param>
        /// <param name="Property"></param>
        /// <param name="Default"></param>
        /// <returns></returns>
        public static TValue Get<TValue>(this JObject Json, string Property, TValue Default = default)
        {
            if (Json.Has(Property))
            {
                try { return Json.Value<TValue>(Property); }
                catch
                {
                }
            }

            return Default;
        }

        /// <summary>
        /// Get the value.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="Json"></param>
        /// <param name="Property"></param>
        /// <param name="Default"></param>
        /// <returns></returns>
        public static bool TryGet<TValue>(this JObject Json, string Property, out TValue OutValue)
        {
            if (Json.Has(Property))
            {
                try
                {
                    OutValue = Json.Value<TValue>(Property);
                    return true;
                }
                catch
                {
                }
            }

            OutValue = default;
            return false;
        }

        /// <summary>
        /// Convert an object to JSON object.
        /// </summary>
        /// <param name="Any"></param>
        /// <returns></returns>
        public static JObject ToJson(this object Any)
            => JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(Any));
    }
}
