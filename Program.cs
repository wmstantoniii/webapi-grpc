using grpc1.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
    .SetMinimumLevel(LogLevel.Trace)
    .AddConsole());
var logger = loggerFactory.CreateLogger<Program>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var webapiPort = 8080;
var webapiProtocols = HttpProtocols.Http1;
 
var grpcPort = 8081;

// hack for mac: Setup HTTP/2 endpoint without TLS
// https://learn.microsoft.com/en-us/aspnet/core/grpc/troubleshoot?view=aspnetcore-7.0#unable-to-start-aspnet-core-grpc-app-on-macos
var grpcProtocols = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
    ? HttpProtocols.Http2
    : HttpProtocols.Http1AndHttp2;

builder.WebHost.ConfigureKestrel(options =>
{
    // webapi
    options.ListenLocalhost(webapiPort, o =>
    {
        o.Protocols = webapiProtocols;
    });
    // grpc: 
    options.ListenLocalhost(grpcPort, o =>
    {
        o.Protocols = grpcProtocols;
    });
});

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcReflectionService();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

logger.LogInformation($"listening on port {webapiPort} for webapi");
logger.LogInformation($"listening on port {grpcPort} for grpc");

app.Run();
