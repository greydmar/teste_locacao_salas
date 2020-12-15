using System;
using System.Linq;
using System.Threading.Tasks;
using locacao.clientebd;
using locacao.clientebd.DTO;
using locacao.tests.Internal;
using Microsoft.EntityFrameworkCore;

namespace locacao.tests.DataContext.EfContext
{
    /// <summary>
    /// Fixture Contexto EfCore
    /// </summary>
    public abstract class TestContextoLocacaoSalas: AbstractDisposable
    {
        private readonly DbContextOptions<ContextoLocacaoSalas> _contextOptions;
        private static readonly object _lock = new object();
        private static bool _initialized;


        protected TestContextoLocacaoSalas(DbContextOptions<ContextoLocacaoSalas> options)
        {
            _contextOptions = options;

            Seed();
        }


        public async Task Execute(Func<ContextoLocacaoSalas, Task> contextAction)
        {
            using (var ctx = CreateContext())
            {
                await contextAction(ctx);
            }
        }

        public async Task<TResult> Execute<TResult>(Func<ContextoLocacaoSalas, Task<TResult>> contextAction)
        {
            using (var ctx = CreateContext())
            {
                return await contextAction(ctx);
            }
        }

        public ContextoLocacaoSalas CreateContext()
        {
            return new ContextoLocacaoSalas(_contextOptions);
        }

        private void Seed()
        {
            lock (_lock)
            {
                if (_initialized)
                    return;
                
                using (var ctx = new ContextoLocacaoSalas(_contextOptions))
                {
                    ctx.Database.EnsureDeleted();
                    ctx.Database.EnsureCreated();

                    RegistrarSalas(ctx);
                    RegistrarUsuarios(ctx);

                    ctx.SaveChanges(true);
                    
                    _initialized = true;
                }
            }
        }

        private void RegistrarUsuarios(ContextoLocacaoSalas ctx)
        {
            var usuarios = AuxiliarDados.UsuariosAmostra;
            ctx.ListaUsuarios.AddRange(usuarios);
        }

        private void RegistrarSalas(ContextoLocacaoSalas ctx)
        {
            var salas = AuxiliarDados.SalasDisponiveis;
            var lista = salas.Select(sala => new PerfilSalaReuniaoInterno()
            {
                Grupo = sala.Grupo,
                Identificador = sala.Identificador,
                QuantidadeAssentos = sala.QuantidadeAssentos,
                Recursos = sala.Recursos
            });
            
            ctx.ListaSalas.AddRange(lista);
        }

        public void ChecarInicializacao()
        {
            Seed();
        }
    }
}