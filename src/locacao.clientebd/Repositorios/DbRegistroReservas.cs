using System.Collections.Generic;
using System.Threading.Tasks;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces;
using mtgroup.locacao.Interfaces.Repositorios;

namespace locacao.clientebd
{
    internal class DbRegistroReservas : DbConsultaReservas, IRepositorioReservas
    {
        public DbRegistroReservas(ContextoLocacaoSalas ctx) : base(ctx) { }

        public async Task<ReservaSalaReuniao> GravarReserva(ReservaSalaReuniao reservaSala)
        {
            var dbReservas = GetDbSet<ReservaSalaReuniao>();

            var entry = await dbReservas.AddAsync(reservaSala);

            DbCtx.SaveChanges();

            return entry.Entity;
        }
    }
}