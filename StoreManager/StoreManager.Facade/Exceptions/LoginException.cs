namespace StoreManager.Facade.Exceptions;

public class LoginException : Exception
{
    public LoginException(string username) : base("Login failed")
    {
        Username = username ?? throw new ArgumentNullException(nameof(username));
    }

    public string Username { get; }

    public DateTime Date => DateTime.Now;
}
