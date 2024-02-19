
namespace GrpcDemo.WebApi;

using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using GrpcEmailer;
using System.Net;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddGrpcClient<Email.EmailClient>(o =>
            {
                o.Address = new Uri("static://localhost");
            })
            .ConfigureChannel(o =>
            {
                o.Credentials = ChannelCredentials.Insecure;
                o.ServiceConfig = new ServiceConfig()
                {
                    LoadBalancingConfigs = { new RoundRobinConfig() }
                };
            });

        builder.Services.AddSingleton<ResolverFactory>(
            sp => new StaticResolverFactory(_ =>
            [
                new BalancerAddress("localhost", 5158),
                new BalancerAddress("localhost", 32769)
            ]));

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapPost("/email", async (Email.EmailClient emailClient, EmailRequest request) =>
        {
            var response = await emailClient.SendEmailAsync(request);

            return TypedResults.Created();
        });

        app.Run();
    }
}
