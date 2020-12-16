using System.Threading.Tasks;
using FluentResults;
using mtgroup.auth.DataModel;
using mtgroup.auth.Interfaces.Model;

namespace mtgroup.auth.Interfaces
{
    public interface IConsultaUsuarios
    {
        Task<Usuario> Localizar(RequisicaoAutenticacaoUsuario criterio);
    }
    
    public interface IServicoAutorizacao
    {
        Task<Result<UsuarioAutorizado>> Autenticar(RequisicaoAutenticacaoUsuario model);
    }
}