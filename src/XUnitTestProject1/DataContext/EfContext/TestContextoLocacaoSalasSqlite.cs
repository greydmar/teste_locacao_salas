using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using locacao.clientebd;
using locacao.clientebd.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using XUnitTestProject1.Specs;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace locacao.tests.DataContext.EfContext
{
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
    }

    public class TestContextoLocacaoSalasSqlite : TestContextoLocacaoSalas
    {
        public TestContextoLocacaoSalasSqlite() : 
            base(new DbContextOptionsBuilder<ContextoLocacaoSalas>()
                .UseSqlite("FileName=TestContextoLocacaoSalas.db")
                /*.UseLoggerFactory(CreateFactory())
                .EnableSensitiveDataLogging()*/
                .Options)
        {
        }

        private static ILoggerFactory CreateFactory()
        {
            var factory = LoggerFactory.Create(builder=> builder.AddConsole());
            return factory;
        }
    }
    
    
}
