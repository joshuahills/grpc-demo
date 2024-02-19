namespace GrpcServer;

using GrpcServer.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddGrpc();
        // builder.Services.AddGrpcReflection();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<EmailService>();
        //app.MapGrpcReflectionService();

        app.Run();
    }
}