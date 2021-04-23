using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DPRHSE.WebAPI.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection RegisterServices<T>(this IServiceCollection services)
        {
            var assemblyserviseToScan = Assembly.GetAssembly(typeof(T));
            services.RegisterAssemblyPublicNonGenericClasses(assemblyserviseToScan)
            .Where(x => x.Name.EndsWith("Service"))
            .AsPublicImplementedInterfaces();

            return services;
        }
    }
}
