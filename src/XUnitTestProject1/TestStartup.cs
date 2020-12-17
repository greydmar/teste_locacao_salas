using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using mtgroup.locacao.DataContext;
using mtgroup.locacao.DataContext.EfContext;
using mtgroup.locacao.Interfaces;
using mtgroup.locacao.Interfaces.Repositorios;
using mtgroup.locacao.Interfaces.Servicos;
using mtgroup.locacao.Repositorios;
using mtgroup.locacao.Servicos;

namespace mtgroup.locacao
{
    public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IServicoDataHora, ServicoDataHora>();
            
            services.AddScoped<IValidacaoRequisicao, ServicoValidacaoRequisicao>();
            services.AddScoped<IServicoAgendamento, ServicoAgendamento>();

            services.AddScoped<IRepositorioReservas>(NsubstituteHelper.RepositorioReservas);
            services.AddScoped<IConsultaReservas>(NsubstituteHelper.ConsultaReservas);
            services.AddScoped<IContextoExecucao>(NsubstituteHelper.ContextoExecucao);

            ////var descriptor = services.SingleOrDefault
            ////    (d => d.ServiceType == typeof(DbContextOptions<ContextoLocacaoSalas>));

            ////if (descriptor != null)
            ////    services.Remove(descriptor);
            
            ////services.AddDbContext<ContextoLocacaoSalas>(TestContextoLocacaoSalasSqlite.SetupOptions);
            ////services.AddSingleton<TestContextoLocacaoSalas, TestContextoLocacaoSalasSqlite>();

            /* Chamada da fixture para garantir a inicialização do db-context*/
            //var provider = services.BuildServiceProvider();

            //var fixture = provider.GetRequiredService<TestContextoLocacaoSalas>();

            //fixture.ChecarInicializacao();
            //TestContextoLocacaoSalas
        }
    }
}
