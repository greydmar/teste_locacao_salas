using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Ag3.Util.Mensagens
{
    public static class ResultadoOperacaoExtensions
    {
        public static IEnumerable<TResultadoOperacao> EhAceitavel<TResultadoOperacao, TEntrada, TOcorrencia>(
            this IEnumerable<TResultadoOperacao> source,
            Func<TResultadoOperacao, bool> teste)
            where TResultadoOperacao: AbstractResultado<TEntrada, TOcorrencia>
            where TEntrada: class
            where TOcorrencia: class, IOcorrenciaOperacao
        {
            foreach (var resultado in source)
            {
                resultado.Aceitavel = teste(resultado);
            }

            return source;
        }

        public static bool TemOcorrencia<TResultadoOperacao, TEntrada, TOcorrencia>(this TResultadoOperacao origem, Func<TOcorrencia, bool> criteria)
            where TResultadoOperacao: ResultadoOperacao<TEntrada>
            where TEntrada: class
            where TOcorrencia: class, IOcorrenciaOperacao
        {
            var rStatus = origem.Ocorrencia;

            if (rStatus is TOcorrencia targetStatus)
                return criteria(targetStatus);

            if (rStatus is ColecaoOcorrenciaOperacao targetStatus2)
                return targetStatus2.HasOne(criteria);

            throw new NotSupportedException(
                $"Não é possível converter o status de \"{nameof(origem)}\" para \"{typeof(TOcorrencia)}\"");
        }

        public static IOcorrenciaComStatus<int, TStatus> OcorrenciaComStatus<TResultadoOperacao, TEntrada, TStatus>(
            this TResultadoOperacao origem, Func<TStatus, bool> criteria)

            where TResultadoOperacao: ResultadoOperacao<TEntrada>
            where TEntrada: class
            where TStatus: IComparable
        {
            var rStatus = origem.Ocorrencia;

            if (rStatus is IOcorrenciaComStatus<int, TStatus> targetStatus && criteria(targetStatus.Status))
            {
                return targetStatus;
            }

            if (rStatus is IColecaoStatusOperacao colecao)
            {
                var match = colecao.Values.FirstOrDefault(ocorrencia =>
                {
                    return ocorrencia is IOcorrenciaComStatus<int, TStatus> targetStatus 
                           && criteria(targetStatus.Status);
                });

                return (IOcorrenciaComStatus<int, TStatus>)match;
            }

            return null;
        }

        public static bool TemOcorrenciaComStatus<TResultadoOperacao, TEntrada, TStatus>(
            this TResultadoOperacao origem, Func<TStatus, bool> criteria)

            where TResultadoOperacao: ResultadoOperacao<TEntrada>
            where TEntrada: class
            where TStatus: IComparable
        {
            return origem.OcorrenciaComStatus<TResultadoOperacao, TEntrada, TStatus>(criteria) != null;
        }


        public static bool TodosTemOcorrencia<TResultadoOperacao, TEntrada, TOcorrencia>(this TResultadoOperacao origem, Func<TOcorrencia, bool> criteria)
            where TResultadoOperacao: ResultadoOperacao<TEntrada>
            where TEntrada: class
            where TOcorrencia: class, IOcorrenciaOperacao
        {
            var rStatus = origem.Ocorrencia;

            if (rStatus is TOcorrencia targetStatus)
                return criteria(targetStatus);

            if (rStatus is ColecaoOcorrenciaOperacao targetStatus2)
                return targetStatus2.All(criteria);

            return false;
        }

        public static DescritivoOcorrencia Consolidar<TResultado, TEntrada>(this IEnumerable<TResultado> origem)
            where TResultado: ResultadoOperacao<TEntrada> 
            where TEntrada : class
        {
            if (origem == null)
                throw new ArgumentNullException(nameof(origem));

            if (!origem.Any())
                throw new ArgumentException("Não há nenhum item para consolidar", nameof(origem));

            if (origem.Count() == 1)
                return origem.First().Ocorrencia;

            var consolidado = new Dictionary<IChaveOcorrencia, DescritivoOcorrencia>();
            foreach (var resultado in origem)
            {
                var status = resultado.Ocorrencia;
                if (!(status is IColecaoStatusOperacao colecao))
                {
                    status.Adicional = new ValueTuple<string, bool?>(nameof(resultado.Aceitavel), resultado.Aceitavel);
                    consolidado.Add(new ChaveOcorrencia(nameof(TEntrada), consolidado.Count), status);
                }
                else
                {
                    foreach (var kvp in colecao)
                    {
                        var item = kvp.Value;
                        item.Adicional ??= kvp.Key;

                        consolidado.Add(kvp.Key, kvp.Value);
                    }
                }
            }

            if (consolidado.Count == 1)
                return consolidado.First().Value;

            return new ColecaoOcorrenciaOperacao(0,null, consolidado);
        }

        public static DescritivoOcorrencia Consolidar(this IEnumerable<DescritivoOcorrencia> origem)
        {
            if (origem == null)
                throw new ArgumentNullException(nameof(origem));

            if (!origem.Any())
                throw new ArgumentException("Não há nenhum item para consolidar", nameof(origem));

            if (origem.Count() == 1)
                return origem.First();

            var consolidado = new Dictionary<IChaveOcorrencia, DescritivoOcorrencia>();
            foreach (var ocorrencia in origem)
            {
                if (!(ocorrencia is IColecaoStatusOperacao colecao))
                {
                    ocorrencia.Adicional = new ValueTuple<string, bool?>(nameof(ocorrencia.Aceitavel), ocorrencia.Aceitavel);
                    consolidado.Add(new ChaveOcorrencia(nameof(TEntrada), consolidado.Count), ocorrencia);
                }
                else
                {
                    foreach (var kvp in colecao)
                    {
                        var item = kvp.Value;
                        item.Adicional ??= kvp.Key;

                        consolidado.Add(kvp.Key, kvp.Value);
                    }
                }
            }

            if (consolidado.Count == 1)
                return consolidado.First().Value;

            return new ColecaoOcorrenciaOperacao(0, null, consolidado);
        }

        public static DescritivoOcorrencia Consolidar<TEntrada>(this IEnumerable<ResultadoOperacao<TEntrada>> origem)
            where TEntrada : class
        {
            return Consolidar<ResultadoOperacao<TEntrada>, TEntrada>(origem);
        }

    }
}