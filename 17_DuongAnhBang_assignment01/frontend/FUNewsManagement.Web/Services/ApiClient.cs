using System.Text;
using System.Text.Json;

namespace FUNewsManagement.Web.Services
{
    public interface IApiClient
    {
        Task<T> GetAsync<T>(string endpoint, string token = null);
        Task<T> PostAsync<T>(string endpoint, object data, string token = null);
        Task<string> PostRawAsync(string endpoint, object data, string token = null);
        Task<T> PutAsync<T>(string endpoint, object data, string token = null);
        Task<string> PutRawAsync(string endpoint, object data, string token = null);
        Task<T> DeleteAsync<T>(string endpoint, string token = null);
        Task<string> DeleteRawAsync(string endpoint, string token = null);
    }

    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiClient(IConfiguration configuration)
        {
            _baseUrl = configuration["ApiSettings:BaseUrl"]?.TrimEnd('/') ?? "https://localhost:5001";
            _httpClient = new HttpClient { BaseAddress = new Uri(_baseUrl + "/") };
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private void SetAuthHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
        }

        public async Task<T> GetAsync<T>(string endpoint, string token = null)
        {
            SetAuthHeader(token);
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }

        public async Task<T> PostAsync<T>(string endpoint, object data, string token = null)
        {
            SetAuthHeader(token);
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
        }

        public async Task<string> PostRawAsync(string endpoint, object data, string token = null)
        {
            SetAuthHeader(token);
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<T> PutAsync<T>(string endpoint, object data, string token = null)
        {
            SetAuthHeader(token);
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(endpoint, content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
        }

        public async Task<string> PutRawAsync(string endpoint, object data, string token = null)
        {
            SetAuthHeader(token);
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(responseContent);
            }
            return responseContent;
        }

        public async Task<T> DeleteAsync<T>(string endpoint, string token = null)
        {
            SetAuthHeader(token);
            var response = await _httpClient.DeleteAsync(endpoint);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(responseContent);
            }
            return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
        }

        public async Task<string> DeleteRawAsync(string endpoint, string token = null)
        {
            SetAuthHeader(token);
            var response = await _httpClient.DeleteAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(content);
            }
            return content;
        }
    }
}
