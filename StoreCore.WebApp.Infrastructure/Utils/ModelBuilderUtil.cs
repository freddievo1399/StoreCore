using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Hosting;
using StoreCore.WebApp.Infrastructure.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StoreCore.WebApp.Infrastructure
{
    public static class ModelBuilderUtil
    {
        public static void RegisterInternal<TEntity>(this ModelBuilder modelBuilder) where TEntity : EntityBase
        {
            var type = typeof(TEntity);
            var tableAttribute = type.GetCustomAttribute<TableAttribute>();
            if (tableAttribute == null)
            {
                throw new InvalidOperationException("Not config Table Name");
            }
            var entityName = type.Name;
            modelBuilder.Entity<TEntity>()
            .HasIndex(c => c.Guid)
            .IsUnique();
            foreach (var property in type.GetProperties())
            {
                var propertyType = property.PropertyType;
                var propertyName = property.Name;
                switch (propertyType)
                {
                    case Type t when t == typeof(decimal) || t == typeof(decimal?):
                        var precisionAttribute = property.GetCustomAttribute<PrecisionAttribute>();
                        if (precisionAttribute == null)
                        {
                            modelBuilder.Entity<TEntity>().Property(propertyName).HasPrecision(18, 2);
                        }
                        break;
                    case Type t when t == typeof(string):
                        var maxLengthAttribute = property.GetCustomAttribute<MaxLengthAttribute>();
                        if (maxLengthAttribute == null)
                        {
                            modelBuilder.Entity<TEntity>().Property(propertyName).HasMaxLength(256);
                        }
                        break;
                    case Type t when t.IsEnum:
                        modelBuilder.Entity<TEntity>().Property(propertyName).HasConversion<string>();
                        break;

                    case Type t when (t.IsClass && t != typeof(string)) || typeof(IEnumerable).IsAssignableFrom(t):
                        {

                            var jsonAttribute = property.GetCustomAttribute<SaveWithJsonAttribute>();
                            if (jsonAttribute != null)
                            {
                                modelBuilder.Entity<TEntity>().OwnsOne(propertyType, propertyName, ownedNavigationBuilder =>
                                            {
                                                ownedNavigationBuilder.ToJson((jsonAttribute.JsonName ?? propertyName) + "JsonData");
                                            });
                            }
                            else if (typeof(EntityBase).IsAssignableFrom(propertyType))
                            {
                                var noForeignKeyAttribute = property.GetCustomAttribute<NoForeignKeyAttribute>();
                                var referenceCollectionBuilder = modelBuilder.Entity<TEntity>()
                                        .HasOne(propertyType, propertyName)
                                        .WithMany()
                                        .HasForeignKey($"{propertyType.Name}_Guid")
                                        .HasPrincipalKey("Guid");

                                if (noForeignKeyAttribute != null)
                                {
                                    referenceCollectionBuilder.IsRequired(false)
                                        .OnDelete(DeleteBehavior.NoAction);
                                }
                            }
                            else if (typeof(IEnumerable).IsAssignableFrom(t))
                            {
                                var targetType = t.IsGenericType ? t.GetGenericArguments()[0] : null;

                                if (targetType != null && typeof(EntityBase).IsAssignableFrom(targetType))
                                {
                                    var noForeignKeyAttribute = property.GetCustomAttribute<NoForeignKeyAttribute>();
                                    var referenceCollectionBuilder = modelBuilder.Entity<TEntity>()
                                            .HasMany(targetType, propertyName)
                                            .WithOne()
                                            .HasForeignKey($"{targetType.Name}_Guid")
                                            .HasPrincipalKey("Guid");
                                    if (noForeignKeyAttribute != null)
                                    {
                                        referenceCollectionBuilder
                                            .IsRequired(false)
                                            .OnDelete(DeleteBehavior.NoAction);
                                    }
                                }
                                else
                                {
                                    throw new NotSupportedException();
                                }
                            }
                            else
                            {
                                throw new InvalidOperationException("NotConfig");
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            modelBuilder.Entity<TEntity>().ToTable(tableAttribute.Name);
        }
    }
}
