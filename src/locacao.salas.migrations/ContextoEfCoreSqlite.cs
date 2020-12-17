using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using mtgroup.auth;
using mtgroup.locacao;

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
        public const string ConexaoBdArquivo = "FileName=TestContextoLocacaoSalas.db";
        public const string ConexaoBdTesteIntegracao = "Datasource=file::memory:?cache=shared";
        
        private static readonly string InMemorySqlDatabaseName = $"{Guid.NewGuid():N}.db";
        
        public static void SetupOptions(DbContextOptionsBuilder dbCtxBuilder)
        {
            dbCtxBuilder.UseSqlite(
                    ConexaoBdArquivo,
                    builder =>
                    {
                        builder.MigrationsAssembly(typeof(ContextoEfCoreSqlite).Assembly.GetName().Name);
                    })

                /*.EnableSensitiveDataLogging()
                .UseLoggerFactory(CreateFactory())*/
                ;
        }

        public static void SetupTesteIntegracaoOptions(DbContextOptionsBuilder dbCtxBuilder)
        {
            //FIX SUGERIDO: https://stackoverflow.com/a/47751630
            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = InMemorySqlDatabaseName,
                Mode = SqliteOpenMode.Memory,
                Cache = SqliteCacheMode.Shared
            };

            var connection = new SqliteConnection(builder.ConnectionString);
            connection.Open();
            connection.EnableExtensions(true);
            
            dbCtxBuilder.UseSqlite(connection,
                    builder =>
                    {
                        builder.MigrationsAssembly(typeof(ContextoEfCoreSqlite).Assembly.GetName().Name);
                    })
                /*.EnableSensitiveDataLogging()
                .UseLoggerFactory(CreateFactory())*/
                ;
        }

        private static readonly object _lock = new object();

        public static IHost InicializarBdSqLiteTesteIntegracao(this IHost host)
        {
            return InicializarBdSqLite(host, true);
        }
        
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
    }
}
