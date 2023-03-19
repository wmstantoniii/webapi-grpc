using grpc1.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var webapiProtocols = HttpProtocols.Http1;
var grpcProtocols = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
    ? HttpProtocols.Http2 // hack for mac: Setup HTTP/2 endpoint without TLS
    : HttpProtocols.Http1AndHttp2;

builder.WebHost.ConfigureKestrel(options =>
{
    // webapi
    options.ListenLocalhost(8080, o => o.Protocols = webapiProtocols);
    // grpc: 
    options.ListenLocalhost(8081, o => o.Protocols = grpcProtocols);
});

builder.Services.AddGrpc();

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
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
