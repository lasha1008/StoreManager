using Newtonsoft.Json;
using StoreManager.Web.Interfaces;
using System.Net.Http.Headers;
using System.Text;

namespace StoreManager.Web.Helpers;

public class ApiHelper : IApiHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly string _apiBaseAddress;
    private string? _token;

    public ApiHelper(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _apiBaseAddress = configuration["ApiEndpoints:BaseAddress"] ?? throw new InvalidOperationException("ApiSettings:BaseAddress is not configured");
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _token = _httpContextAccessor.HttpContext?.Session?.GetString("Token") ?? null;
    }

    public string? Token => _token;

    public async Task<(bool IsSuccess, T? Data)> SendRequest<T>(string endpoint, HttpMethod method, HttpContent? content = null)
    {
        using (var httpClient = new HttpClient { BaseAddress = new Uri(_apiBaseAddress) })
        {
            if (!string.IsNullOrEmpty(_token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            }

            var response = await httpClient.SendAsync(new HttpRequestMessage(method, endpoint) { Content = content });

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<T>(responseData);
                return (true, data);
            }

            return (false, default);
        }
    }

    public async Task<bool> Login(string username, string password)
    {
        var loginData = new
        {
            Username = username,
            Password = password
        };

        var loginContent = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

        using (var httpClient = new HttpClient { BaseAddress = new Uri(_apiBaseAddress) })
        {
            var response = await httpClient.PostAsync(_configuration["ApiEndpoints:Login"], loginContent);

            if (response.IsSuccessStatusCode)
            {
                _httpContextAccessor?.HttpContext?.Session.SetString("Token", await response.Content.ReadAsStringAsync());
                return true;
            }

            return false;
        }
    }
}
