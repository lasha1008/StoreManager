using System.Text.Json;

namespace StoreManager.API.ExceptionHandler;

internal sealed record ErrorDetails(int StatusCode, string Message)
{
    public override string ToString() => JsonSerializer.Serialize(this);
}
