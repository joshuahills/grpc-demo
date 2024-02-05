namespace GrpcClient;

using Grpc.Net.Client;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using GrpcEmailerClient;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    static async Task Main()
    {
        var factory = new StaticResolverFactory(_ =>
        [
            new BalancerAddress("localhost", 5158),
            new BalancerAddress("localhost", 32769)
        ]);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<ResolverFactory>(factory);

        using var channel = GrpcChannel.ForAddress(
            "static://localhost",
            new GrpcChannelOptions
            {
                Credentials = Grpc.Core.ChannelCredentials.Insecure,
                ServiceConfig = new ServiceConfig
                {
                    LoadBalancingConfigs = { new RoundRobinConfig() },
                },
                ServiceProvider = serviceCollection.BuildServiceProvider(),
            });
        var client = new Email.EmailClient(channel);

        while (true)
        {
            Console.Write("\nEnter the email address: ");
            var address = Console.ReadLine();

            Console.Write("Enter the subject: ");
            var subject = Console.ReadLine();

            Console.Write("Enter the message: ");
            var message = Console.ReadLine();

            var reply = await client.SendEmailAsync(new EmailRequest{ Address = address, Subject = subject, Content = message });

            Console.WriteLine(reply);
        }
    }
}
