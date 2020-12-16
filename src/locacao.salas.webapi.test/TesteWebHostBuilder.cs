using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace mtgroup.locacao
{
    public sealed class TesteWebHostBuilder
    {
        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            
            //new WebHostBuilder().Conf
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<WebTesteStartup>();
                });

    }
}