using locacao.clientebd;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace locacao.tests.DataContext.EfContext
{
    using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

    public class TestContextoLocacaoSalasSqlite : TestContextoLocacaoSalas
    {
        public TestContextoLocacaoSalasSqlite() : 
            base(ResolveDbOptions())
        {
        }

        public static DbContextOptions<ContextoLocacaoSalas> ResolveDbOptions()
        {
            var sb = new DbContextOptionsBuilder<ContextoLocacaoSalas>();
            SetupOptions(sb);
            return sb.Options;
        }

        private static ILoggerFactory CreateFactory()
        {
            var factory = LoggerFactory.Create(builder=>
            {
                builder.AddConsole()
                    .SetMinimumLevel(LogLevel.Debug);
            });
            return factory;
        }

        public static void SetupOptions(DbContextOptionsBuilder dbCtxBuilder)
        {
            dbCtxBuilder
                .UseSqlite("FileName=TestContextoLocacaoSalas.db")
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(CreateFactory())
                
                ;
        }
    }
    
    
}
