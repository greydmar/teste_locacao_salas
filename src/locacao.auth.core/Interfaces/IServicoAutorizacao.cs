using System.Security.Claims;
using System.Threading.Tasks;
using FluentResults;
using locacao.auth.core.DataModel;

namespace locacao.auth.core.Interfaces
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