using Microsoft.EntityFrameworkCore;
using StoreCore.WebApp.Abstractions;
using StoreCore.WebApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.Product.Server
{
    [Index(nameof(Name), IsUnique = true)]
    [Table("PRODUCT")]
    public class ProductEntity : EntityBase
    {
        [Required]
        public required string Name { get; set; }

        public Result Result { get; set; }
    }
}
