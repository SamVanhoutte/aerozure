using Microsoft.Extensions.DependencyInjection;

namespace Aerozure.Communication;

public static class ServiceCollectionExtensions
{
    public static void AddCommunicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICommunicationService, SmsCommunicationService>();
        services.AddScoped<ICommunicationService, WhatsappCommunicationService>();
        services.AddScoped<ICommunicationService, MailCommunicationService>();
    }
}