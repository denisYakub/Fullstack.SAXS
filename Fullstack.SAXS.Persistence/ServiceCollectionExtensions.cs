using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Domain.Abstractions.Handlers;
using Fullstack.SAXS.Domain.Abstractions.Services;
using Fullstack.SAXS.Persistence.HTML;
using Fullstack.SAXS.Persistence.IO;
using Fullstack.SAXS.Persistence.Kafka;
using Fullstack.SAXS.Persistence.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fullstack.SAXS.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFileService(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CsvOptions>(config);

            services
                .AddScoped<IFileService, FileService>();
        }

        public static void AddGraphService(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<GraphOptions>(config);

            services
                .AddHostedService<PythonProcessHostedService>()
                .AddScoped<IGraphService, GraphService>();
        }

        public static void AddProducer<TMessage>(this IServiceCollection services, IConfigurationSection config)
        {
            services.Configure<KafkaOptions>(config);

            services
                .AddSingleton<IProducer<TMessage>, KafkaProducer<TMessage>>();
        }

        public static void AddConsumer<TMessage, THandler>(this IServiceCollection services, IConfigurationSection config)
            where THandler : class, IMessageHandler<TMessage>
        {
            services.Configure<KafkaOptions>(config);

            services
                .AddScoped<IMessageHandler<TMessage>, THandler>()
                .AddHostedService<KafkaConsumer<TMessage>>();
        }
    }
}
