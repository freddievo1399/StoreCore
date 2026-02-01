using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.WebApp.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entityRegisters = AssembliesUtil.GetAssemblies().GetInstances<IRegisterServer>();
            foreach (var i in entityRegisters)
            {
                i.RegisterEntities(modelBuilder);
            }
            foreach (var modelType in modelBuilder.Model.GetEntityTypes())
            {
                var nameTable = modelType.FindAnnotation("Relational:TableName");
                var entity = modelBuilder.Entity(modelType.Name);
                if (nameTable != null)
                {
                    entity.ToTable(nameTable.Value?.ToString());
                }

                var props = entity.Metadata.GetDeclaredProperties();
                foreach (IMutableProperty prop in props)
                {

                    var propEntity = entity.Property(prop.Name);
                    var clrType = prop.ClrType;
                    if (clrType == typeof(decimal) || clrType == typeof(decimal?))
                    {
                        var Precision = prop.FindAnnotation(nameof(PrecisionAttribute));

                        if (Precision == null)
                        {
                            propEntity.HasPrecision(18, 6);
                        }
                        continue;
                    }
                }

            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
