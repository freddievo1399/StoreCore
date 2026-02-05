using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.WebApp.Infrastructure.Database.Interface
{
    public interface IWorkSession : IQuerySession
    {
        void Add<T>(T entity) where T : EntityBase;
        void AddRange<T>(IEnumerable<T> entities) where T : EntityBase;

        void Remove<T>(T entity) where T : EntityBase;

        void RemoveRange<T>(IEnumerable<T> entities) where T : EntityBase;

        Task SaveAsync();
    }
}
