using ZoneTree.Blazor.Common.Domain.Models;
using ZoneTree.Blazor.Common.Domain.Services;
using ZoneTree.Blazor.Common.Infrastructure.Helpers;

namespace ZoneTree.BlazorServer.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class GithubService
    {
        private readonly ICacheService<List<ApiResponse>> _cacheService;
        private const string _key_base = nameof(GithubService);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheService"></param>
        public GithubService(ICacheService<List<ApiResponse>> cacheService)
        {
            _cacheService = cacheService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<ApiResponse>> GetGithubCommits()
        {
            string key = $"{_key_base}_{nameof(GetGithubCommits)}";
            if (_cacheService.Exists(key))
                return _cacheService.Get(key);

            var (isSuccess, response) = await HttpHelper.GetTAsync<List<ApiResponse>>();
            if (!isSuccess || response == null)
                return new List<ApiResponse>();

            _cacheService.Set(key, response);
            return response;
        }
    }
}
