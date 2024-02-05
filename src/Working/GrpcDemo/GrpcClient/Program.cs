namespace GrpcClient;

using Grpc.Net.Client;
using GrpcEmailerClient;

internal class Program
{
    static async Task Main()
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5158");
        var client = new Email.EmailClient(channel);

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
