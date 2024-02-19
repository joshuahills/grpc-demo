namespace GrpcDemo.WebApi;

using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using GrpcEmailer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddLogging(o => o.AddConsole())
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
            app.Logger.LogInformation("Sending email: {@emailReq}", request);
            var response = await emailClient.SendEmailAsync(request);
            app.Logger.LogInformation("Received success: {success}", response.Success);

            return TypedResults.Created();
        });

        app.Run();
    }
}
