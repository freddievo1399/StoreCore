using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using StoreCore.WebApp.Infrastructure.Database.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.WebApp.Infrastructure.Database;

internal class WorkSession : QuerySession, IWorkSession
{
    private readonly List<string> _lockKeys;
    private readonly List<SemaphoreSlim> _semaphores;
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _pool;
    private IDbContextTransaction? _transaction;
    private bool _isCommitted = false;

    public WorkSession(ApplicationDbContext context, List<string> lockKeys, List<SemaphoreSlim> semaphores, ConcurrentDictionary<string, SemaphoreSlim> pool)
        : base(context)
    {
        _lockKeys = lockKeys;
        _semaphores = semaphores;
        _pool = pool;
        // Bật Tracking để EF tự phát hiện thay đổi trên Entity
        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
    }

    public void Add<T>(T entity) where T : EntityBase => _context.Set<T>().Add(entity);
    public void AddRange<T>(IEnumerable<T> entities) where T : EntityBase => _context.Set<T>().AddRange(entities);

    public override IQueryable<T> Query<T>() where T : class => _context.Set<T>().AsTracking<T>();


    public void Remove<T>(T entity) where T : EntityBase => _context.Set<T>().Remove(entity);
    public void RemoveRange<T>(IEnumerable<T> entities) where T : EntityBase => _context.Set<T>().RemoveRange(entities);

    public async Task SaveAsync()
    {
        _transaction ??= await _context.Database.BeginTransactionAsync();
        await _context.SaveChangesAsync();
        await _transaction.CommitAsync();
        _isCommitted = true;
    }

    public override async ValueTask DisposeAsync()
    {
        try
        {
            // Tự động Rollback nếu quên SaveAsync hoặc văng lỗi
            if (_transaction != null && !_isCommitted) await _transaction.RollbackAsync();
            if (_transaction != null) await _transaction.DisposeAsync();
            await base.DisposeAsync();
        }
        finally
        {
            for (int i = 0; i < _lockKeys.Count; i++)
            {
                var key = _lockKeys[i];
                var semaphore = _semaphores[i];

                semaphore.Release();
                if (semaphore.CurrentCount == 1) _pool.TryRemove(key, out _);
            }
        }
    }
}
