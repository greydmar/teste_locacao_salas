using System.Threading.Tasks;
using locacao.auth.core.DataModel;
using locacao.auth.core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace mtgroup.auth.Repositorios
{
    public class DbConsultaUsuarios: DbContextRepo, IConsultaUsuarios
    {
        public DbConsultaUsuarios(ContextoAutorizacao ctx) 
            : base(ctx)
        { }

        public async Task<Usuario> Localizar(RequisicaoAutenticacaoUsuario criterio)
        {
            return await DbCtx.ListaUsuarios
                .SingleOrDefaultAsync(item => item.NomeLogin == criterio.Username);
        }
    }
}