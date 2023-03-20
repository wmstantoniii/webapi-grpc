# webapi grpc mashup
> This app was created by combining apps generated from dotnet templates:
> - dotnet new grpc
> - dotnet new webapi

> ### to use this code on MacOS
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

