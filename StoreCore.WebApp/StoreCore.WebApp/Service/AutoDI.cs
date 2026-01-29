using RestEase;
using StoreCore.WebApp.Abstractions;

namespace StoreCore.WebApp
{
    public class AutoDI<T>(T service) : IAppService<T>
    {
        private async Task<V> ExcuteInternal<V>(Func<T, Task<V>> func) where V : Result, new()
        {

            return await func(service);
        }
        public async Task<Result> Excute(Func<T, Task<Result>> func)
        {
            return await ExcuteInternal(func);
        }

        public async Task<ResultOf<V>> Excute<V>(Func<T, Task<ResultOf<V>>> func)
        {
            return await ExcuteInternal(func);
        }

        public async Task<ResultsOf<V>> Excute<V>(Func<T, Task<ResultsOf<V>>> func)
        {
            return await ExcuteInternal(func);
        }
        public async Task<PagedResultsOf<V>> Excute<V>(Func<T, Task<PagedResultsOf<V>>> func)
        {
            return await ExcuteInternal(func);
        }
    }
}
