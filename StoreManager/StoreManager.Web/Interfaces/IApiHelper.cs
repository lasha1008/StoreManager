namespace StoreManager.Web.Interfaces;

public interface IApiHelper
{
    Task<(bool IsSuccess, T? Data)> SendRequest<T>(string endpoint, HttpMethod method, HttpContent? content = null);

    Task<bool> Login(string username, string password);
}
