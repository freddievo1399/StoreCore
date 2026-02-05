using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using StoreCore.WebApp.Infrastructure.Database.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.WebApp.Infrastructure.Database;

internal class QuerySession : IQuerySession
{
    protected readonly ApplicationDbContext _context;
    public QuerySession(ApplicationDbContext context)
    {
        _context = context;
        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public virtual IQueryable<T> Query<T>() where T : class => _context.Set<T>().AsNoTracking();

    public async Task<TValue?> SqlAsync<TValue>(FormattableString sql)
        => await _context.Database.SqlQuery<TValue>(sql).FirstOrDefaultAsync();

    public async Task<TValue?> ExecuteAsync<TValue>(Func<DatabaseFacade, Task<TValue?>> action)
    {
        return await action(_context.Database);
    }

    public virtual async ValueTask DisposeAsync() => await _context.DisposeAsync();
}
