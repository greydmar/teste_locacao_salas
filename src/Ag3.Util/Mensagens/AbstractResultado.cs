using System;

namespace Ag3.Util.Mensagens
{
    /// <summary>
    /// O resultado de uma operação é aceitável se a ocorrência associada à operação é nula ou aceitável
    /// ou se o atributo aceitável foi explicitamente definido como aceitavel
    /// </summary>
    /// <typeparam name="TEntrada"></typeparam>
    /// <typeparam name="TOcorrencia"></typeparam>
    public abstract class AbstractResultado<TEntrada, TOcorrencia>: IResultadoOperacao<TEntrada>
        where TEntrada: class
        where TOcorrencia: class, IOcorrenciaOperacao
    {
        private bool? _resultadoAceitavel = null;
        private TOcorrencia _ocorrencia;

        protected AbstractResultado(TEntrada entrada, TOcorrencia ocorrencia)
        {
            Entrada = entrada;
            _ocorrencia = ocorrencia;
        }

        protected AbstractResultado(TEntrada entrada)
        {
            Entrada = entrada;
            _ocorrencia = default;
        }

        public TEntrada Entrada { get; }

        public bool? Aceitavel
        {
            get
            {
                if (_resultadoAceitavel == null)
                    return TestarSucessoInterno() ?? false;

                return _resultadoAceitavel ?? false;
            }

            internal set
            {
                if (value == default)
                    throw new InvalidOperationException("Aceitável não deve ser nulo");

                _resultadoAceitavel = value;
            }
        }

        IOcorrenciaOperacao IResultadoOperacao.Ocorrencia => Ocorrencia;

        public virtual TOcorrencia Ocorrencia => _ocorrencia;

        private bool? TestarSucessoInterno()
        {
            return TestarSucesso()
                   ?? !(Ocorrencia?.HaErros());
        }
        
        protected abstract bool? TestarSucesso();

        protected virtual void DefinirAceitavel(Func<TOcorrencia, bool> criterio)
        {
            bool? resultado = null;
            try
            {
                resultado = criterio(this.Ocorrencia);
            }
            finally
            {
                _resultadoAceitavel = resultado;
            }
        }
    }
}