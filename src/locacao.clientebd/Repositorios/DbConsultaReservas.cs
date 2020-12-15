using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using locacao.clientebd.DTO;
using Microsoft.EntityFrameworkCore;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces;
using mtgroup.locacao.Interfaces.Repositorios;

namespace locacao.clientebd
{
    internal class DbConsultaReservas: DbContextRepo, IConsultaReservas
    {
        public DbConsultaReservas(ContextoLocacaoSalas ctx) 
            : base(ctx)
        {
        }

        public async Task<bool> ExistePerfilSala(RequisicaoSalaReuniao requisicao)
        {
            return GetDbSet<PerfilSalaReuniaoInterno>()
                .Any(perfil => perfil.QuantidadeAssentos >= requisicao.QuantidadePessoas
                               && (perfil.Recursos & requisicao.Recursos) != 0
                );
        }

        public async Task<IEnumerable<IPerfilSalaReuniao>> ListarSalasDisponiveis(PeriodoLocacao periodo)
        {
            var salasDisponiveis = GetDbSet<PerfilSalaReuniaoInterno>();

            var reservadas = GetDbSet<ReservaSalaReuniao>()
                .Where(reserva => reserva.Periodo.Inicio >= periodo.Inicio)
                .Select(reserva => reserva.IdSalaReservada);

            return salasDisponiveis.Where(sala =>
                !reservadas.Contains(sala.Identificador, StringComparer.OrdinalIgnoreCase)
            );
        }
    }
}