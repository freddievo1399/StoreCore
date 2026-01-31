using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.WebApp.Infrastructure
{
    public interface IRegisterServer
    {
        public void RegisterEntities(ModelBuilder modelbuilder);
        public void RegisterServices(IServiceCollection services);
    }
}
