using Microsoft.EntityFrameworkCore;
using mtgroup.auth.DataModel;
using mtgroup.auth.Interfaces;
using mtgroup.auth.Mapeamentos;

namespace mtgroup.auth
{
    public partial class ContextoAutorizacao: DbContext
    {
        public DbSet<Usuario> ListaUsuarios => this.Set<Usuario>();

        protected ContextoAutorizacao()
        {
        }

        public ContextoAutorizacao(DbContextOptions<ContextoAutorizacao> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioConfiguracao());
            
            base.OnModelCreating(modelBuilder);
        }
    }

    public abstract class DbContextRepo
    {
        protected readonly ContextoAutorizacao DbCtx;

        protected DbContextRepo(ContextoAutorizacao ctx)
        {
            this.DbCtx = ctx;
        }

        protected DbSet<TEntity> GetDbSet<TEntity>()
            where TEntity : class, IEntidade
        {
            return DbCtx.Set<TEntity>();
        }
    }

}