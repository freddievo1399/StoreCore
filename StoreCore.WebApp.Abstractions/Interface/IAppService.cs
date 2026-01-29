using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.WebApp.Abstractions;

public interface IAppService<T>
{
    public Task<Result> Excute(Func<T, Task<Result>> func);

    Task<ResultOf<V>> Excute<V>(Func<T, Task<ResultOf<V>>> func);

    public Task<ResultsOf<V>> Excute<V>(Func<T, Task<ResultsOf<V>>> func);
    public Task<PagedResultsOf<V>> Excute<V>(Func<T, Task<PagedResultsOf<V>>> func);
}
