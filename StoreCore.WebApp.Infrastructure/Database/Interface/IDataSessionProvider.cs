using StoreCore.WebApp.Infrastructure.Database.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.WebApp.Infrastructure
{
    public interface IDataSessionProvider
    {
        Task<IQuerySession> OpenQueryAsync();
        Task<IWorkSession> OpenWorkAsync(params string[] lockKeys);
    }
}
