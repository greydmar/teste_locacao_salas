using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Ag3.Util.Mensagens
{
    public interface IChaveOcorrencia: ITuple, IStructuralComparable, IStructuralEquatable
    {
        IEnumerable<IComparable> Partes { get; }
    }

    public interface IColecaoStatusOperacao : IOcorrenciaOperacao,
        IReadOnlyDictionary<IChaveOcorrencia, DescritivoOcorrencia>
    {

    }

    public class ColecaoOcorrenciaOperacao : DescritivoOcorrencia, IColecaoStatusOperacao
    {
        private readonly ImmutableDictionary<IChaveOcorrencia, DescritivoOcorrencia> _ocorrencias;

        public ColecaoOcorrenciaOperacao(int codigo, string status, 
            IDictionary<IChaveOcorrencia, DescritivoOcorrencia> itens) 
            : base(codigo, status)
        {
            _ocorrencias = itens.ToImmutableDictionary();
        }

        #region IDictionary

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<IChaveOcorrencia, DescritivoOcorrencia>> GetEnumerator()
        {
            return this._ocorrencias.GetEnumerator();
        }

        public int Count => _ocorrencias.Count;

        public bool ContainsKey(IChaveOcorrencia key)
        {
            return _ocorrencias.ContainsKey(key);
        }

        public bool TryGetValue(IChaveOcorrencia key, out DescritivoOcorrencia value)
        {
            return _ocorrencias.TryGetValue(key, out value);
        }

        public DescritivoOcorrencia this[IChaveOcorrencia key] => _ocorrencias[key];

        public IEnumerable<IChaveOcorrencia> Keys => _ocorrencias.Keys;

        public IEnumerable<DescritivoOcorrencia> Values => _ocorrencias.Values;

        #endregion

        public override bool HaErros()
        {
            return base.HaErros() || 
                   this._ocorrencias.Any(i => i.Value.HaErros());
        }

        public bool HasOne<TInfoStatus>(Func<TInfoStatus, bool> predicate)
            where TInfoStatus: IOcorrenciaOperacao
        {
            return _ocorrencias.Values.OfType<TInfoStatus>().Count(predicate) >= 1;
        }

        public bool All<TInfoStatus>(Func<TInfoStatus, bool> predicate)
            where TInfoStatus: IOcorrenciaOperacao
        {
            return _ocorrencias.Values.OfType<TInfoStatus>().Count(predicate) == _ocorrencias.Count;
        }
    }
}