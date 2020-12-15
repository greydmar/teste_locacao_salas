using locacao.clientebd.DTO;
using locacao.clientebd.Mapeamentos;
using Microsoft.EntityFrameworkCore;
using mtgroup.locacao.DataModel;
using mtgroup.locacao.Interfaces;

namespace locacao.clientebd
{
    public partial class ContextoLocacaoSalas: DbContext
    {
        public DbSet<PerfilSalaReuniaoInterno> ListaSalas => this.Set<PerfilSalaReuniaoInterno>();

        public DbSet<Solicitante> ListaUsuarios => this.Set<Solicitante>();

        protected ContextoLocacaoSalas()
        {
        }

        public ContextoLocacaoSalas(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PerfilSalaReuniaoConfiguracao());
            modelBuilder.ApplyConfiguration(new ReservaSalaReuniaoConfiguracao());
            modelBuilder.ApplyConfiguration(new SolicitanteConfiguracao());
            
            base.OnModelCreating(modelBuilder);
        }
    }

    public abstract class DbContextRepo
    {
        protected readonly ContextoLocacaoSalas DbCtx;

        protected DbContextRepo(ContextoLocacaoSalas ctx)
        {
            this.DbCtx = ctx;
        }

        protected DbSet<TEntity> GetDbSet<TEntity>()
            where TEntity: class, IEntidade
        {
            return DbCtx.Set<TEntity>();
        }
    }
}
