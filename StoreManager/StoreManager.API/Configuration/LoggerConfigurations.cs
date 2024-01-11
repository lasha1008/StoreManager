using Serilog.Events;
using Serilog;

namespace StoreManager.API.Configuration;

public static class LoggerConfigurations
{
    public static void ConfigureLogger(this Serilog.ILogger logger)
    {
        Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Information()
           .WriteTo.File("logs/log-.txt",
                restrictedToMinimumLevel: LogEventLevel.Warning)
           .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
           .CreateLogger();
    }
}
