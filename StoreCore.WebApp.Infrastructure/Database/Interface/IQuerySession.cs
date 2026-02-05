
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.WebApp.Infrastructure
{
    public interface IQuerySession : IAsyncDisposable
    {
        IQueryable<T> Query<T>() where T : class;

        Task<TValue?> SqlAsync<TValue>(FormattableString sql);

        Task<TValue?> ExecuteAsync<TValue>(Func<DatabaseFacade, Task<TValue?>> action);
    }
}
