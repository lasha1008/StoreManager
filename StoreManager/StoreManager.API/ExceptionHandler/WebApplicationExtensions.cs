using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Net;

namespace StoreManager.API.ExceptionHandler;

public static class WebApplicationExtensions
{
    public static void HandleException(this WebApplication app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    Log.Error(contextFeature.Error, "Application Error");
                    await context.Response.WriteAsync(
                        new ErrorDetails(context.Response.StatusCode, "Internal Server Error."
                        ).ToString());
                }
            });
        });
    }
}
