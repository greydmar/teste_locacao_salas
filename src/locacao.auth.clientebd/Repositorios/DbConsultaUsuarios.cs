using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mtgroup.auth.DataModel;
using mtgroup.auth.Interfaces;
using mtgroup.auth.Interfaces.Model;

namespace mtgroup.auth.Repositorios
{
    public class DbConsultaUsuarios: DbContextRepo, IConsultaUsuarios
    {
        public DbConsultaUsuarios(ContextoAutorizacao ctx) 
            : base(ctx)
        { }

        public async Task<Usuario> Localizar(RequisicaoAutenticacaoUsuario criterio)
        {
            try
            {
                return await DbCtx.ListaUsuarios
                    .SingleOrDefaultAsync(item => item.NomeLogin == criterio.Login);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}