using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Cobranca.Util.IoC
{
    public static class Registro
    {
        public static void RegistrarServicos(IServiceCollection services, Type baseType)
        {
            var baseAssembly = AppDomain.CurrentDomain.Load(baseType.Assembly.GetName());
            var concretes = baseAssembly.GetTypes()
                .Where(t => t.IsClass &&
                            t.IsSubclassOf(baseType));

            foreach (var concrete in concretes)
            {
                foreach (var @interface in concrete.GetInterfaces().Where(x => !x.Name.Contains("Base")))
                {
                    services.AddScoped(@interface, concrete);
                }
            }
        }
    }
}