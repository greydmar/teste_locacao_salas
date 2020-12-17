using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using mtgroup.locacao.DTO;
using mtgroup.locacao.Internal;

namespace mtgroup.locacao.DataContext.EfContext
{
    sealed class CreateTableSqlRaw
    {
        public const string RawSqlUsuarioConectado =
            "CREATE TABLE sistema_usuario_conectado ( " +
            "\"Id\" INTEGER NOT NULL CONSTRAINT \"PK_sistema_usuario_conectado\" PRIMARY KEY AUTOINCREMENT," +
            "\"Nome\" TEXT NOT NULL, " +
            "\"SobreNome\" TEXT NULL, " +
            "   \"nomeLogin\" TEXT NOT NULL, " +
            "\"Password\" TEXT NOT NULL " +
            ");\n" +

            "INSERT INTO \"sistema_usuario_conectado\"(\"Id\", \"Nome\", \"nomeLogin\", \"Password\", \"SobreNome\") " +
            "VALUES(1, 'Usuario01', 'Usuario01', 'AQAAAAEAACcQAAAAEIXu9RKNPhAJKmUTnwJ5ASsI/nrTC5qOPrfIH8WvIhWrDG7ct0XVuCfhUCAuJAqiaA==', NULL);\n" +

            "CREATE UNIQUE INDEX \"sistema_usuario_identificador\" ON \"sistema_usuario_conectado\"(\"Id\");\n" +
            
            "CREATE UNIQUE INDEX \"sistema_usuario_nome\" ON \"sistema_usuario_conectado\"(\"nomeLogin\");"
            ;
    }

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
                    var deleted = ctx.Database.EnsureDeleted();
                    var created = ctx.Database.EnsureCreated();
                    RegistrarUsuarios(ctx);

                    ctx.SaveChanges(true);
                }

                using (var ctx = new ContextoLocacaoSalas(_contextOptions))
                {
                    var created = ctx.Database.EnsureCreated();

                    RegistrarSalas(ctx);

                    ctx.SaveChanges(true);

                    _initialized = true;
                }

            }
        }

        private void RegistrarUsuarios(ContextoLocacaoSalas ctx)
        {
            // criação e preenchimento de tabela de usuários
            var rawSqlUsuarioConectado = FormattableStringFactory.Create(CreateTableSqlRaw.RawSqlUsuarioConectado);
            var rowCount = ctx.Database.ExecuteSqlRaw(rawSqlUsuarioConectado.ToString(CultureInfo.InvariantCulture));
            //var usuarios = AuxiliarDados.UsuariosAmostra;
            //ctx.ListaUsuarios.AddRange(usuarios);
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