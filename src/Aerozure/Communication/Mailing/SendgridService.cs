using Aerozure.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Aerozure.Communication.Mailing;

public class SendgridService : IMailService
{
    private readonly SendGridSettings sendGridSettings;

    public SendgridService(IOptions<SendGridSettings> settings)
    {
        sendGridSettings = settings.Value;
    }
    
    public async Task<CommResult> SendTemplatedMailAsync(string recipientMail, string? recipientName, string template, object mergeContent)
    {
        var client = new SendGridClient(sendGridSettings.ApiKey);

        var msg = MailHelper.CreateSingleTemplateEmail(
            new EmailAddress(sendGridSettings.SenderAddress, sendGridSettings.SenderName),
            new EmailAddress(recipientMail,recipientName),
            template, mergeContent);

        var response = await client.SendEmailAsync(msg);
        return CommResult.FromResponse(response);
    }

    public async Task<CommResult> SendTemplatedMailsAsync(string template, List<Email> emails)
    {
        var client = new SendGridClient(sendGridSettings.ApiKey);

        var msgs = MailHelper.CreateMultipleTemplateEmailsToMultipleRecipients(
            new EmailAddress(sendGridSettings.SenderAddress, sendGridSettings.SenderName),
            emails.Select(mail => new EmailAddress(mail.RecipientAddress, mail.RecipientName)).ToList(),
            template, emails.Select(mail => mail.MergeContent).ToList());

        var response = await client.SendEmailAsync(msgs);
        var responseContent = await response.Body.ReadAsStringAsync();
        return CommResult.FromResponse(response);
    }

    public async Task<CommResult> SendMailAsync(string recipientAddress, string recipientName, string subject, string body)
    {
        var client = new SendGridClient(sendGridSettings.ApiKey);

        var from = new EmailAddress(sendGridSettings.SenderAddress, sendGridSettings.SenderName);
        var to = new EmailAddress(recipientAddress,recipientName);
        var plainTextContent = body;
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent,
            $"<html><body>{body}</body></html>");
        var response = await client.SendEmailAsync(msg);
        return CommResult.FromResponse(response);
    }
    
}