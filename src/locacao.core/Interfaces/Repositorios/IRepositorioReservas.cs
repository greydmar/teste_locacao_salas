using System.Threading.Tasks;
using mtgroup.locacao.DataModel;

namespace mtgroup.locacao.Interfaces.Repositorios
{
    public interface IRepositorioReservas: IConsultaReservas
    {
        Task<ReservaSalaReuniao> GravarReserva(ReservaSalaReuniao reservaSala);
    }
}