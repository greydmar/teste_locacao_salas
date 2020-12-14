using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Cobranca.Util;
using Newtonsoft.Json;

namespace Ag3.Util
{
    public sealed class ColecaoFixa<TColecao, TItem>
    {
        private ColecaoFixa() { }

        private static ImmutableHashSet<TItem> _valores;

        public static IImmutableSet<TItem> Todos
        {
            get
            {
                InicializarSeNecessario();
                return _valores;
            }
        }

        private static void InicializarSeNecessario()
        {
            if (_valores != null)
                return;

            var valores = typeof(TColecao)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(fInfo => fInfo.FieldType == typeof(TItem))
                .Select(f => (TItem) f.GetValue(null))
                .ToImmutableHashSet();

            InterlockedUtils.ThreadSafeReplacement(ref _valores, () => valores);
        }
    }

    public delegate void WriteJsonDelegate<T>(JsonWriter writer, T value, JsonSerializer serializer);

    public abstract class WriteOnlyJsonConverter<T> : JsonConverter<T>
    {
        //private readonly WriteJsonDelegate<T> _serializer;

        //protected WriteOnlyJsonConverter([NotNull] WriteJsonDelegate<T> serializer)
        //{
        //    _serializer = serializer;
        //}

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public sealed override bool CanRead { get; } = false;
    }
}