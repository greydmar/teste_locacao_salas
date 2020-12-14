using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Ag3.Util.Mensagens
{
    /// <summary>
    /// Informativo de evento ocorrido em uma ação associada a <see cref="ResultadoOperacao{TEntrada}"/>
    /// </summary>
    /// <typeparam name="TCod"></typeparam>
    /// <typeparam name="TStatus"></typeparam>
    public abstract class AbstractInfoOcorrencia<TCod, TStatus>: IOcorrenciaComStatus<TCod, TStatus>
        where TCod: IComparable<TCod>
        where TStatus: IComparable
    {
        private TStatus _status;
        private TCod _codigo;

        protected AbstractInfoOcorrencia(TCod codigo, string mensagens, TStatus status, Exception exception = null)
        {
            _codigo = codigo;
            _status = status;
            Mensagens = ((string.IsNullOrEmpty(mensagens) || mensagens.Length == 0) && exception == null)
                ? ImmutableArray<string>.Empty
                : CombinarMensagens(mensagens, exception).ToImmutableArray();
            Exception = exception;
        }

        public abstract bool HaErros();

        public virtual TStatus Status
        {
            get => _status;
            set => _status = value;
        }

        public virtual TCod Codigo
        {
            get => _codigo;
            set => _codigo = value;
        }

        public IEnumerable<string> Mensagens { get; set; }

        public Exception Exception { get; }

        private static IEnumerable<string> CombinarMensagens(string mensagens, Exception exception)
        {
            List<string> result = new List<string>();

            if (!string.IsNullOrEmpty(mensagens))
                result.Add(mensagens);

            if (exception != null)
            {
                var inner = exception;
                while (inner!=null)
                {
                    if (!string.IsNullOrEmpty(inner.Message))
                        result.Add($"[{inner.GetType().Name}]:\"{inner.Message}");

                    inner = inner.InnerException;
                }
            }

            return result;
        }
    }
}