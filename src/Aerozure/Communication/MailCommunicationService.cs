using Aerozure.Communication.Context;
using Aerozure.Communication.Mailing;

namespace Aerozure.Communication;

public class MailCommunicationService : ICommunicationService
{
    private readonly IMailService mailService;

    public MailCommunicationService(IMailService mailService)
    {
        this.mailService = mailService;
    }

    public async Task<CommunicationContext> SendAsync(CommunicationContext context, Func<CommunicationContext, Task>? loggingTask = null)
    {
        if (context.Email?.Recipient != null)
        {
            string body = context.Email.Body;
            CommResult? commResult = null;
            if (string.IsNullOrEmpty(context.Email.TemplateId))
            {
                // Send a simple email with body & content
                if (!string.IsNullOrEmpty(context.Email.Body))
                {
                    commResult = await mailService.SendMailAsync(context.Email.Recipient.Address,
                        context.Email.Recipient.Name,
                        context.Subject, context.Email.Body);
                }
            }
            else
            {
                // Use structured data to send templated email
                body = context.Email.TemplateId;
                commResult = await mailService.SendTemplatedMailAsync(context.Email.Recipient.Address,
                    context.Email.Recipient.Name,
                    context.Email.TemplateId, context.StructuralData);
            }

            context.Body = body;
            context.Email.Result = commResult;
            if(loggingTask != null)
            {
                await loggingTask(context);
            }
        }

        return context;
    }
}