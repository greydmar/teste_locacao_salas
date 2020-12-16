using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using mtgroup.db;

namespace mtgroup.locacao
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .InicializarBdSqLite(false)
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
