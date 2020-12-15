using System.Collections.Generic;
using System.Collections.Immutable;
using FluentResults;

namespace mtgroup.locacao.DataModel
{
    public class ResultadoReservaSala : Result<ReservaSalaReuniao>
    {
        private ResultadoReservaSala(List<Reason> reasons)
        {
            base.WithReasons(reasons);
        }
        
        public ResultadoReservaSala() { }

        public ResultadoReservaSala(Reason ocorrencia, bool? ehAceitavel = null) { }


        public ImmutableList<IPerfilSalaReuniao> Sugestoes { get; private set; }
        
        public static ResultadoReservaSala Falhou(Result falha, string mensagemErro)
        {
            var tmpResult = new ResultadoReservaSala(falha.Reasons);
            if (string.IsNullOrEmpty(mensagemErro))
                mensagemErro = "Sem detalhes adicionais da falha!";
            tmpResult.WithError(mensagemErro);
            return tmpResult;
        }

        public static ResultadoReservaSala FalhouComSugestoes(
            Result<RequisicaoSalaReuniao> falha,
            IEnumerable<IPerfilSalaReuniao> lista)
        {
            var result = Falhou(falha, null);

            result.Sugestoes = lista.ToImmutableList();

            return result;
        }

        public static ResultadoReservaSala OK(ReservaSalaReuniao reservaSala)
        {
            var result = new ResultadoReservaSala();
            result.WithValue(reservaSala);
            return result;
        }
    }
}