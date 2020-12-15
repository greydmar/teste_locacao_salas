using System.Linq;
using locacao.clientebd;
using locacao.clientebd.Repositorios;
using locacao.tests.DataContext;
using locacao.tests.DataContext.EfContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using mtgroup.locacao.Interfaces;
using mtgroup.locacao.Interfaces.Repositorios;
using mtgroup.locacao.Interfaces.Servicos;
using mtgroup.locacao.Servicos;

namespace locacao.tests
{
    public class TestStartup
    {
        public void ConfigureService(IServiceCollection services)
        {
            services.AddSingleton<IServicoDataHora, ServicoDataHora>();
            
            services.AddScoped<IValidacaoRequisicao, ServicoValidacaoRequisicao>();
            services.AddScoped<IServicoAgendamento, ServicoAgendamento>();

            services.AddScoped<IRepositorioReservas,DbRegistroReservas>();
            services.AddScoped<IConsultaReservas, DbConsultaReservas>();
            services.AddScoped<IContextoExecucao>(NsubstituteHelper.ContextoExecucao);

            var descriptor = services.SingleOrDefault
                (d => d.ServiceType == typeof(DbContextOptions<ContextoLocacaoSalas>));

            if (descriptor != null)
                services.Remove(descriptor);
            
            services.AddDbContext<ContextoLocacaoSalas>(TestContextoLocacaoSalasSqlite.SetupOptions);
            services.AddSingleton<TestContextoLocacaoSalas, TestContextoLocacaoSalasSqlite>();

            /* Chamada da fixture para garantir a inicialização do db-context*/
            var provider = services.BuildServiceProvider();

            var fixture = provider.GetRequiredService<TestContextoLocacaoSalas>();

            fixture.ChecarInicializacao();
            //TestContextoLocacaoSalas
        }
    }
}
