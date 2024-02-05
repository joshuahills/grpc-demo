# gRPC Demo

This is a demo of a .NET gRPC server with .NET and Go gRPC clients.

The demo mimics an email microservice but doesn't do anything aside from log things to the console.

## Running the demo

### Server instances

The C# client has client-side load balancing enabled for two server instances, so you need two instances running on the hardcoded ports (5158 and 32769).

You can use the docker-compose project to spin up a Docker container to host one of the server instances and `dotnet run -lp http` in a terminal for another instance.

This is handy for demo-ing the round robin load balancing.

### Clients

The clients take input from stdin to send to the server.

#### .NET

The .NET client can be run simply with `dotnet run`.

#### Go

From the directory containing the built executable, simply run it:

```bash
./email.exe
```