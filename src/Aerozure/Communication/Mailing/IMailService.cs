namespace Aerozure.Communication.Mailing;

public interface IMailService
{
    Task<CommResult> SendTemplatedMailAsync(string recipientAddress, string? recipientName, string template, object? mergeContent);
    Task<CommResult> SendTemplatedMailsAsync(string template, List<Email> emails);
    Task<CommResult> SendMailAsync(string recipientAddress, string? recipientName, string subject, string body);
}