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
    
    public interface IServicoAutenticacao
    {
        Task<Result<RespostaUsuarioAutorizado>> Autenticar(RequisicaoAutenticacaoUsuario model);
    }
}