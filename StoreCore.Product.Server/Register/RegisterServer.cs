using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StoreCore.WebApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.Product.Server
{
    public class RegisterServer : IRegisterServer
    {
        public void RegisterEntities(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<ProductEntity>();
            modelbuilder.Entity<ProductEntity>()
            .OwnsOne(x=>x, Result =>
            {
                // Change JSON property name from "StreetName" to "street_name"
                Result.Property(a => a.Result)
                       .HasJsonPropertyName("JS");
            });
        }

        public void RegisterServices(IServiceCollection services)
        {
            throw new NotImplementedException();
        }
    }
}
