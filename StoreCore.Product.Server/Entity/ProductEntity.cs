using Microsoft.EntityFrameworkCore;
using StoreCore.WebApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.Product.Server.Entity
{
    [Index(nameof(Name), IsUnique = true)]

    public class ProductEntity : EntityBase
    {
        public required string Name { get; set; }
    }
}
