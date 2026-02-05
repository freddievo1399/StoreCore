using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.WebApp.Infrastructure
{
    public static class AutoRegisterDI
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            var serviceRegisters = AssembliesUtil.GetAssemblies().GetInstances<IRegisterServer>();
            foreach (var i in serviceRegisters)
            {
                i.RegisterServices(services);
            }
        }
    }
}
