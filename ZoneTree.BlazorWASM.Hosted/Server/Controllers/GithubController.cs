using Microsoft.AspNetCore.Mvc;
using ZoneTree.Blazor.Common.Domain.Models;
using ZoneTree.Blazor.Common.Domain.Services;
using ZoneTree.Blazor.Common.Infrastructure.Helpers;

namespace ZoneTree.BlazorWASM.Hosted.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GithubController : ControllerBase
    {
        private readonly ICacheService<List<ApiResponse>> _cacheService;
        private const string _key_base = nameof(GithubController);

        public GithubController(ICacheService<List<ApiResponse>> cacheService)
        {
            _cacheService = cacheService;
        }

        /// <summary>
        /// Get github commits
        /// </summary>
        /// <returns></returns>
        [HttpGet("commits")]
        public async Task<IActionResult> GetGithubCommits()
        {
            string key = $"{_key_base}_{nameof(GetGithubCommits)}";
            if (_cacheService.Exists(key))
                return Ok(_cacheService.Get(key));

            var (isSuccess, response) = await HttpHelper.GetTAsync<List<ApiResponse>>();
            if (!isSuccess || response == null)
                return Ok(new List<ApiResponse>());

            _cacheService.Set(key, response);
            return Ok(response);
        }
    }
}
