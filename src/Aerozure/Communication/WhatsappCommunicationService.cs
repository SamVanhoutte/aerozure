using Aerozure.Communication.Context;
using Aerozure.Configuration;
using Aerozure.Templating;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Aerozure.Communication;

public class WhatsappCommunicationService : ICommunicationService
{
    private readonly TwilioSettings twilioSettings;

    public WhatsappCommunicationService(IOptions<TwilioSettings> settings)
    {
        twilioSettings = settings.Value;
    }

    public async Task<CommunicationContext> SendAsync(CommunicationContext context, Func<CommunicationContext, Task>? loggingTask = null)
    {
        try
        {
            var body = await GetTextBodyAsync(context);
            if (string.IsNullOrEmpty(context.WhatsApp?.PhoneNumber) || string.IsNullOrEmpty(body))
            {
                context.WhatsApp ??= new WhatsAppContext();
                context.WhatsApp.Result = CommResult.NotSent;
                return context;
            }
            // Initialize the Twilio client
            TwilioClient.Init(twilioSettings.AccountSid, twilioSettings.AuthToken);

            // Send a new outgoing SMS by POSTing to the Messages resource
            var messageResource = await MessageResource.CreateAsync(
                from: new PhoneNumber(twilioSettings.PhoneNumber), // From number, must be an SMS-enabled Twilio number
                to: new PhoneNumber(context.Sms.PhoneNumber), // To number, if using Sandbox see note above
                validityPeriod: 10*60, // 10 minutes to attempt delivery
                shortenUrls: true,
                body: body // Message content
            );

            context.Sms.Result = new CommResult
            {
                Success = (messageResource.Status!= MessageResource.StatusEnum.Failed && messageResource.Status!=MessageResource.StatusEnum.Undelivered), 
                StatusCode = messageResource.Status.ToString(), ErrorMessage = messageResource.ErrorMessage, TransactionId = messageResource.Sid
            };
            if(loggingTask != null)
            {
                await loggingTask(context);
            }
            return context;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    private async Task<string?> GetTextBodyAsync(CommunicationContext context)
    {

        if (!string.IsNullOrEmpty(context.Sms.Body)) return context.Sms.Body;
        if (!string.IsNullOrEmpty(context.WhatsApp.Template))
            return await Templater.ParseAsync(context.WhatsApp.Template, context.StructuralData);
        return null;
    }
}