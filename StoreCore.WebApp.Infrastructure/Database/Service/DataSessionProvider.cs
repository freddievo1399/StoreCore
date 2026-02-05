using Microsoft.EntityFrameworkCore;
using StoreCore.WebApp.Infrastructure.Database;
using StoreCore.WebApp.Infrastructure.Database.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.WebApp.Infrastructure
{
    public class DataSessionProvider(IDbContextFactory<ApplicationDbContext> contextFactory) : IDataSessionProvider
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory = contextFactory;
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _lockPool = new();

        public async Task<IQuerySession> OpenQueryAsync()
        {
            var context = await _contextFactory.CreateDbContextAsync();
            return new QuerySession(context);
        }

        public async Task<IWorkSession> OpenWorkAsync(params string[] lockKeys)
        {
            var sortedKeys = lockKeys.Distinct().OrderBy(k => k).ToList();

            var semaphores = new List<SemaphoreSlim>();

            try
            {
                foreach (var key in sortedKeys)
                {
                    var semaphore = _lockPool.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
                    await semaphore.WaitAsync();
                    semaphores.Add(semaphore);
                }

                var context = await _contextFactory.CreateDbContextAsync();
                // Truyền danh sách semaphores vào để lúc Dispose nó nhả hết ra
                return new WorkSession(context, sortedKeys, semaphores, _lockPool);
            }
            catch
            {
                // Nếu lỗi nửa chừng, nhả những cái đã khóa ra
                foreach (var s in semaphores) s.Release();
                throw;
            }
        }
    }
}
