# webapi grpc mashup
> Work in progress.   I'm writing this to relearm .NET core on an open source platform (VSCode on the Mac).  
> The application was created by combining apps generated from dotnet templates:
> - dotnet new grpc
> - dotnet new webapi

> ### to use this code on MacOS (obsolete doc, fixme)
> - brew install grpcurl
> - clone the repo
> - open webapi-gprc.csproj in Rider
> - build it
> - run app in Rider using the grpc configuration
> 
> 
> ### to test the APIs
> #### webapi (runs on port 8080)
> 
```
curl --location 'http://localhost:8080/WeatherForecast'
```
> 
> 
> #### grpc (runs on port 8081) 
```
wstanton-MacBookPro15,1-C02ZJ19JLVDQ:ot wstanton$ grpcurl --plaintext  -d '{ "name": "World" }' localhost:8081 greet.Greeter/SayHello
```
```
{
  "message": "Hello World"
}
```

```
wstanton-MacBookPro15,1-C02ZJ19JLVDQ:ot wstanton$ grpcurl --plaintext localhost:8081 describe
```
```
greet.Greeter is a service:
service Greeter {
  rpc SayHello ( .greet.HelloRequest ) returns ( .greet.HelloReply );
}
grpc.reflection.v1alpha.ServerReflection is a service:
service ServerReflection {
  rpc ServerReflectionInfo ( stream .grpc.reflection.v1alpha.ServerReflectionRequest ) returns ( stream .grpc.reflection.v1alpha.ServerReflectionResponse );
}
```
> #### construction notes
> ##### nuget:
> - **Grpc.AspNetCore**  
> basic functionality, compiling proto files, setting up the grpc service
> - **Grpc.AspNetCore.Server.Reflection**  
> _grpcurl_ wants reflection, otherwise without metadata, it won't work

> ##### 2 ports: 
> 
> - **webapi** (REST, Http1) runs on 8080
> - **grpc** (http2) runs on 8081  
> we have disabled TLS for the grpc port 8081 when running in a devenv
> ###### looks like we may need 2 ports, for .net6/C#.
> https://stackoverflow.com/questions/75562400/running-rest-api-and-grpc-simultanously-using-asp-net-core
> ###### golang lets you multiplex the protocols, i think:
> https://drgarcia1986.medium.com/listen-grpc-and-http-requests-on-the-same-port-263c40cb45ff

>
> ##### add grpc and reflection to the service collection:
```
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
```
> ##### Configure the HTTP request pipeline.
```
app.MapGrpcService<GreeterService>();
app.MapGrpcReflectionService();
```