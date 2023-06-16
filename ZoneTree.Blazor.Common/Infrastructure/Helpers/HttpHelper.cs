using System.Net.Http.Headers;
using System.Text.Json;

namespace ZoneTree.Blazor.Common.Infrastructure.Helpers
{
    public static class HttpHelper
    {
        /// <summary>
        /// Get users
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>        
        public static async Task<(bool isSuccess, T? response)> GetTAsync<T>(string url = "https://api.github.com/users/hadley/orgs")
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("product", "1"));

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode) return (false, default);

            var content = await response.Content.ReadAsStringAsync();

            // simulating the time consumption
            await Task.Delay(5000);
            return (true, JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                WriteIndented = true,
            }));
        }
    }
}
