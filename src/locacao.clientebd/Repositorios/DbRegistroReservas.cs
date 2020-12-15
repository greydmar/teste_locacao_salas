using System.Linq;
using System.Threading.Tasks;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces.Repositorios;

namespace locacao.clientebd.Repositorios
{
    public class DbRegistroReservas : DbConsultaReservas, IRepositorioReservas
    {
        public DbRegistroReservas(ContextoLocacaoSalas ctx) : base(ctx) { }

        public async Task<ReservaSalaReuniao> GravarReserva(ReservaSalaReuniao reservaSala)
        {
            var dbReservas = GetDbSet<ReservaSalaReuniao>();

            // TODO Fix: Necessário verificar problema de update quando utilizando solicitante "desatachado" 
            var usr = DbCtx.ListaUsuarios.FirstOrDefault(solicitante=> solicitante.Name == reservaSala.Solicitante.Name);
            
            reservaSala.Solicitante = usr;

            var entry = await dbReservas.AddAsync(reservaSala);

            DbCtx.SaveChanges();

            return entry.Entity;
        }
    }
}