using RestEase;
using StoreCore.WebApp.Abstractions;

namespace StoreCore.WebApp.Client
{
    public class HttpService<T>(HttpClient httpClient) : IAppService<T>
    {
        readonly T _apiClient = RestClient.For<T>(httpClient);
        private async Task<V> ExcuteInternal<V>(Func<T, Task<V>> func) where V : Result, new()
        {

            try
            {
                var rlt = await func(_apiClient);
                return rlt;
            }
            catch (ApiException ex)
            {
                return new V
                {
                    Success = false,
                    Message = $"{ex.StatusCode}{ex.Content}_{ex.Message}"
                };
            }
            catch (HttpRequestException ex)
            {
                return new V
                {
                    Success = false,
                    Message = $"Server / CORS /network:{ex.StatusCode}_{ex.Message}"
                };
            }
            catch (TaskCanceledException ex)
            {
                return new V
                {
                    Success = false,
                    Message = $"Timeout: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new V
                {
                    Success = false,
                    Message = $"{ex.Message}"
                };
            }
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
