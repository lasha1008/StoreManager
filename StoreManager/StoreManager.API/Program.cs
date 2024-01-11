using Serilog;
using StoreManager.API.Configuration;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

Log.Logger.ConfigureLogger();

builder.Host.UseSerilog();
builder.Services.AddControllers();

builder.ConfigureDependency(configuration);
builder.ConfigureSwagger();
builder.ConfigureAuthentication();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.Configure();
app.Run();
