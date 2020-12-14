using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Ag3.Util
{
    public static partial class JsonUtils
    {
        [ThreadStatic]
        private static readonly JsonSerializerSettings _serializeSettings = DefaultSerializerSettings;

        [ThreadStatic]
        private static readonly JsonSerializerSettings _deserializeSettings = DefaultDeserializerSettings;

        public static JsonSerializerSettings DefaultSerializerSettings
        {
            get
            {
                var result = new JsonSerializerSettings()
                {
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Ignore,
                    Culture = CultureInfo.InvariantCulture,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                result.Converters.Add(new StringEnumConverter()
                {
                    AllowIntegerValues = false
                });

                return result;
            }
        }

        public static JsonSerializerSettings DefaultDeserializerSettings
        {
            get
            {
                var result = new JsonSerializerSettings(){Formatting = Formatting.None};
                result.Converters.Add(new StringEnumConverter() {AllowIntegerValues = true});

                return result;
            }
        }

        public static object Copy<T>(T source)
            where T : class
        {
            var serialized = Serialize(source, _serializeSettings);
            return Deserialize<T>(serialized, _deserializeSettings);
        }

        public static TResult Copy<TResult, TSource>(TSource source)
            where TResult : class
        {
            var serialized = Serialize(source, _serializeSettings);
            return Deserialize<TResult>(serialized, _deserializeSettings);
        }

        public static string Serialize<T>(T source, JsonSerializerSettings settings = null)
        {
            return JsonConvert.SerializeObject(source, settings??_serializeSettings);
        }

        public static TResult Deserialize<TResult>(string source, JsonSerializerSettings deserializeSettings = null)
            where TResult : class
        {
            return JsonConvert.DeserializeObject<TResult>(source, deserializeSettings ?? _deserializeSettings);
        }
    }
}