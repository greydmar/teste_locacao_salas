using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.DTO;
using mtgroup.locacao.Interfaces.Repositorios;

namespace mtgroup.locacao.Repositorios
{
    public class DbConsultaReservas: DbContextRepo, IConsultaReservas
    {
        public DbConsultaReservas(ContextoLocacaoSalas ctx) 
            : base(ctx)
        { }

        public async Task<bool> ExistePerfilSala(RequisicaoSalaReuniao requisicao)
        {
            try
            {
                return await GetDbSet<PerfilSalaReuniaoInterno>().AnyAsync(
                    perfil => perfil.QuantidadeAssentos >= requisicao.QuantidadePessoas
                              && (requisicao.Recursos == RecursoSalaReuniao.Nenhum || (perfil.Recursos & requisicao.Recursos) != 0)
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IEnumerable<IPerfilSalaReuniao>> ListarSalasDisponiveis(PeriodoLocacao periodo)
        {
            var salasDisponiveis = GetDbSet<PerfilSalaReuniaoInterno>();

            var reservadas = GetDbSet<ReservaSalaReuniao>()
                .Where(reserva => reserva.Periodo.Termino > periodo.Inicio)
                .Select(reserva => reserva.IdSalaReservada);

            var dbgResult = salasDisponiveis
                .Where(sala => !reservadas.Contains(sala.Identificador))
                .ToList();

            return dbgResult;
        }
    }
}