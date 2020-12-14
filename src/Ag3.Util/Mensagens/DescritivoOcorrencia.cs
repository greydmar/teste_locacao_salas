using System;

namespace Ag3.Util.Mensagens
{
    /// <summary>
    /// Descritivo de evento ocorrido durante operação em <see cref="ResultadoOperacao{TEntrada}"/>
    /// </summary>
    public class DescritivoOcorrencia : AbstractInfoOcorrencia<int, string>
    {
        public const int CodigoSucessoPadrao = 0;
        public const int CodigoFalhaPadrao = -1;

        public static readonly DescritivoOcorrencia Sucesso = new DescritivoOcorrencia(0, string.Empty, string.Empty);

        public DescritivoOcorrencia(int codigo, string status, 
            string mensagens = null, Exception exception = null) 
            : base(codigo, mensagens, status, exception) { }

        public override bool HaErros()
        {
            return Codigo == CodigoSucessoPadrao
                   && Exception == null;
        }

        /// <summary>
        /// Informação adicional
        /// </summary>
        public object Adicional { get; set; }

        public static DescritivoOcorrencia Novo(bool sucesso, string codigo, string mensagem = null)
        {
            return new DescritivoOcorrencia();
        }
    }

    public class LazyDescritivoOcorrencia : DescritivoOcorrencia
    {
        public LazyDescritivoOcorrencia(
            int codigo, string status, string mensagens = null, Exception exception = null)
            : base(codigo, mensagens, status, exception) { }

    }
}