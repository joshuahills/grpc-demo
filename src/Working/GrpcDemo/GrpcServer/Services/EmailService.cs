namespace GrpcServer.Services;

using Grpc.Core;
using GrpcServer;

public class EmailService(ILogger<EmailService> logger) : Email.EmailBase
{
    public override Task<EmailReply> SendEmail(EmailRequest request, ServerCallContext context)
    {
        logger.LogInformation("""
                Sending email:
                To:      '{address}'
                Subject: '{subject}'
                Content: '{content}'
            """, request.Address, request.Subject, request.Content);
        return Task.FromResult(new EmailReply
        {
            Success = true,
        });
    }
}
