using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using mtgroup.auth;
using mtgroup.locacao;
using mtgroup.locacao.DTO;

namespace mtgroup.db
{
    internal class DesignTimeDbContextFactorySalas : IDesignTimeDbContextFactory<ContextoLocacaoSalas>
    {
        public ContextoLocacaoSalas CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ContextoLocacaoSalas>();
            ContextoEfCoreSqlite.SetupOptions(builder);
         
            return new ContextoLocacaoSalas(builder.Options);
        }
    }

    internal class DesignTimeDbContextFactoryAuth : IDesignTimeDbContextFactory<ContextoAutorizacao>
    {
        public ContextoAutorizacao CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ContextoAutorizacao>();
            ContextoEfCoreSqlite.SetupOptions(builder);
            return new ContextoAutorizacao(builder.Options);
        }
    }

   
    public static class ContextoEfCoreSqlite
    {
        public const string ConexaoArquivoFisico = "FileName=TestContextoLocacaoSalas.db";
        
        public static void SetupOptions(DbContextOptionsBuilder dbCtxBuilder)
        {
            dbCtxBuilder.UseSqlite(
                    ConexaoArquivoFisico,
                    builder =>
                    {
                        builder.MigrationsAssembly(typeof(ContextoEfCoreSqlite).Assembly.GetName().Name);
                    })

                /*.EnableSensitiveDataLogging()
                .UseLoggerFactory(CreateFactory())*/
                ;
        }
        
        private static readonly object _lock = new object();

        public static IHost InicializarBdSqLite(this IHost host, bool reset = false)
        {
            using (var scope = host.Services.CreateScope())
            {
                lock (_lock)
                {
                    var ctxAuth = scope.ServiceProvider.GetRequiredService<ContextoAutorizacao>();
                    var ctxSalas = scope.ServiceProvider.GetRequiredService<ContextoLocacaoSalas>();

                    try
                    {
                        if (reset)
                        {
                            ctxAuth.Database.EnsureDeleted();
                            ctxSalas.Database.EnsureDeleted();
                        }

                        ctxAuth.Database.Migrate();
                        ctxSalas.Database.Migrate();
                        //if (reset)
                        //{
                        //    ctxAuth.Database.EnsureDeleted();
                        //    ctxSalas.Database.EnsureDeleted();
                        //}

                        //var created2 = ctxSalas.Database.EnsureCreatedAsync().Result;
                        //var created1 = ctxAuth.Database.EnsureCreatedAsync().Result;

                        //RegistrarListaSalas(ctxSalas);
                        //RegistrarUsuarios(ctxAuth);
                    }
                    finally
                    {
                        ctxAuth?.Dispose();
                        ctxSalas.Dispose();

                        Task.Delay(500);
                    }
                }
            }

            return host;
        }

        private static void RegistrarUsuarios(ContextoAutorizacao ctx)
        {
            var usuarios = AuxiliarDados.UsuariosAmostra.ToList();
            
            if (ctx.ListaUsuarios.Count()== usuarios.Count)
                return;

            ctx.ListaUsuarios.AddRange(usuarios);

            ctx.SaveChanges(true);
        }

        private static void RegistrarListaSalas(ContextoLocacaoSalas ctx)
        {
            var salas = AuxiliarDados.SalasDisponiveis.ToList();
            
            if (ctx.ListaSalas.Count() == salas.Count)
                return;
            
            var lista = salas.Select(sala => new PerfilSalaReuniaoInterno()
            {
                Grupo = sala.Grupo,
                Identificador = sala.Identificador,
                QuantidadeAssentos = sala.QuantidadeAssentos,
                Recursos = sala.Recursos
            });

            ctx.ListaSalas.AddRange(lista);

            ctx.SaveChanges(true);
        }
    }
}
