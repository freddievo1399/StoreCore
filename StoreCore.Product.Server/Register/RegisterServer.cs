using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using StoreCore.Product.Shared;
using StoreCore.WebApp.Abstractions;
using StoreCore.WebApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.Product.Server
{
    public class RegisterServer : IRegisterServer
    {
        public void RegisterEntities(ModelBuilder modelbuilder)
        {
            modelbuilder.RegisterInternal<ProductEntity>();

        }

        public void RegisterServices(IServiceCollection services)
        {
        }
    }
}
