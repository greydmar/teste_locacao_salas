using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ag3.Util.Mensagens
{
    public class ResultadoOperacao<TEntrada> : AbstractResultado<TEntrada, DescritivoOcorrencia>
        where TEntrada: class
    {
        private readonly Func<TEntrada, (bool, string, string)> _testeAceitacao;
        private readonly Lazy<DescritivoOcorrencia> _lzyOcorrencia = null;

        public ResultadoOperacao(TEntrada entrada, DescritivoOcorrencia ocorrencia, bool? ehAceitavel = null) 
            : base(entrada)
        {
            if (ehAceitavel.HasValue)
                base.Aceitavel = ehAceitavel.Value;

            _lzyOcorrencia = new Lazy<DescritivoOcorrencia>(() => ocorrencia);
        }

        public ResultadoOperacao(TEntrada entrada, 
            Func<TEntrada, (bool, string, string)> testeAceitacao) 
            : base(entrada)
        {
            _testeAceitacao = testeAceitacao;
            _lzyOcorrencia = new Lazy<DescritivoOcorrencia>(AvaliarResultadoAceitavel);
        }

        private DescritivoOcorrencia AvaliarResultadoAceitavel()
        {
            var resultadoTeste = _testeAceitacao(this.Entrada);

            return new DescritivoOcorrencia(
                Convert.ToInt32(resultadoTeste.Item1),
                resultadoTeste.Item2,
                resultadoTeste.Item3
            );
        }


        public sealed override DescritivoOcorrencia Ocorrencia => _lzyOcorrencia.Value;

        protected override bool? TestarSucesso()
        {
            return Ocorrencia?.HaErros();
        }

        public static ResultadoOperacao<TEntrada> Consolidar(ResultadoOperacao<TEntrada> atual, DescritivoOcorrencia falha)
        {
            
            throw new System.NotImplementedException();
        }

        public static ResultadoOperacao<TEntrada> EhAceitavel(string nomeOperacao, TEntrada entrada, 
            Func<TEntrada, (bool, string, string)> testeAceitacao)
        {
            return new ResultadoOperacao<TEntrada>(entrada, testeAceitacao);
        }

        public static ResultadoOperacao<TEntrada> EhAceitavelSeTodos(string nomeOperacao, TEntrada entrada,
            params Func<TEntrada, (bool, string, string)>[] testes)
        {
            var resumo = testes
                .Select(fn => fn(entrada))
                .Select(result=> DescritivoOcorrencia.Novo(result.Item1, result.Item2, result.Item3))
                .ToList()
                .Consolidar();

            return new ResultadoOperacao<TEntrada>(entrada, resumo);
        }
    }
}