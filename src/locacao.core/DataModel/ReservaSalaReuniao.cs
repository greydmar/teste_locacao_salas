using System.Collections.Generic;
using System.Linq;
using FluentResults;
using mtgroup.locacao.Interfaces;

namespace mtgroup.locacao.DataModel
{
    public interface IEntidade
    {
        int Id { get; }

    }
    
    public class ReservaSalaReuniao: IEntidade
    {
        public ReservaSalaReuniao() {}

        public int Id { get; internal set; }

        public string IdSalaReservada { get; set; }

        public Solicitante Solicitante { get; set; }

        public string CodigoReserva { get; set; }

        public PeriodoLocacao Periodo { get; set; }

        public ushort QuantidadePessoas { get; set; }

        ////public ReservaSalaReuniao ComCodigoReserva(string codigo)
        ////{
        ////    return new ReservaSalaReuniao()
        ////    {
        ////        CodigoReserva = codigo,
        ////        Periodo = this.Periodo,
        ////        Solicitante = this.Solicitante,
        ////        QuantidadePessoas = this.QuantidadePessoas,
        ////    };
        ////}

        
    }

    public class ResultadoReservaSala : ResultBase
    {
        public ResultadoReservaSala(ReservaSalaReuniao entrada
            , Reason ocorrencia, bool? ehAceitavel = null) 
        {
        }

        public static ResultadoReservaSala Falhou(Result<RequisicaoSalaReuniao> result, string mensagem)
        {
            throw new System.NotImplementedException();
        }

        public static ResultadoReservaSala FalhouComSugestoes(IEnumerable<IPerfilSalaReuniao> listarSugestoes)
        {
            throw new System.NotImplementedException();
        }

        public static ResultadoReservaSala OK(object reservaSala)
        {
            throw new System.NotImplementedException();
        }
    }

    internal sealed class AuxiliarReservas
    {
        public static IPerfilSalaReuniao EncontrarSalaCompativel(
            IEnumerable<IPerfilSalaReuniao> salas,
            RequisicaoSalaReuniao criterio)
        {
            var candidatas = salas
                .Select(perfil => new
                {
                    perfil,
                    perfil.Recursos,
                    Aproximacao = perfil.QuantidadeAssentos - criterio.QuantidadePessoas
                })
                .Where(perfil => (perfil.Recursos & criterio.Recursos) != 0)
                .OrderBy(perfil => perfil.Aproximacao);

            return candidatas
                .Select(sala => sala.perfil)
                .FirstOrDefault();
        }
    }
}