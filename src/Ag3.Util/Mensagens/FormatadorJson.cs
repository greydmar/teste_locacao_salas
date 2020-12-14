using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Ag3.Util.Mensagens
{
    public static class FormatadorJson
    {
        private static readonly JsonSerializerSettings _jsonSettings = GerarConfigBase();

        private static JsonSerializerSettings GerarConfigBase()
        {
            var settings = JsonUtils.DefaultSerializerSettings;

            settings.ContractResolver = new JsonUtils.IgnoreEmptyEnumerableResolver();

            settings.Converters = new List<JsonConverter>(settings.Converters)
            {
                new ChaveOcorrenciaConverter(),
                new ColecaoOcorrenciaConverter(),
                new InfoStatusOperacaoConverter(),
            };

            return settings;
        }

        private class ChaveOcorrenciaConverter : WriteOnlyJsonConverter<ChaveOcorrencia>
        {
            public override void WriteJson(JsonWriter writer, ChaveOcorrencia value, JsonSerializer serializer)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(nameof(value.Partes));
                serializer.Serialize(writer, value.Partes);

                writer.WriteEndObject();
            }
        }

        private class ColecaoOcorrenciaConverter : WriteOnlyJsonConverter<ColecaoOcorrenciaOperacao>
        {
            public override void WriteJson(JsonWriter writer, ColecaoOcorrenciaOperacao value, JsonSerializer serializer)
            {
                var converter = serializer.Converters
                    .OfType<InfoStatusOperacaoConverter>()
                    .FirstOrDefault() ?? new InfoStatusOperacaoConverter();

                writer.WriteStartObject();
                writer.WritePropertyName("Items");

                writer.WriteStartArray();
                foreach (var statusOperacao in value.Values)
                {
                    converter.WriteJson(writer, statusOperacao, serializer);
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }
        }

        public static string FormatarJson(this InfoOcorrenciaOperacao origem,
            JsonSerializerSettings options = null)
        {
            if (origem == null)
                return null;

            var result = JsonUtils.Serialize(origem, options ?? _jsonSettings);
            
            return result;
        }

        public static string FormatarJson(this IEnumerable<InfoOcorrenciaOperacao> origem, JsonSerializerSettings options = null)
        {
            if (origem == null)
                return null;

            if (!origem.Any())
                return string.Empty;

            object toSerialize = origem;

            if (origem.Count() == 1)
                toSerialize = origem.First();
            
            var result = JsonUtils.Serialize(toSerialize, options ?? _jsonSettings);
            
            return result;
        }
    }

    public sealed class InfoStatusOperacaoConverter: WriteOnlyJsonConverter<InfoOcorrenciaOperacao>
    {
        public override void WriteJson(JsonWriter writer, InfoOcorrenciaOperacao value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(value.Codigo));
            serializer.Serialize(writer, value.Codigo);

            writer.WritePropertyName(nameof(value.Status));
            serializer.Serialize(writer, value.Status);
            
            writer.WritePropertyName(nameof(value.Adicional));
            serializer.Serialize(writer, value.Adicional);
            
            writer.WritePropertyName(nameof(value.Mensagens));
            serializer.Serialize(writer, value.Mensagens);

            writer.WritePropertyName(nameof(value.Exception));
            serializer.Serialize(writer, value.Exception);

            writer.WriteEndObject();
        }
    }

}
