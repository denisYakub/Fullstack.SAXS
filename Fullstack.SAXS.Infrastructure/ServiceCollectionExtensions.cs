using Confluent.Kafka;
using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Infrastructure.HTML;
using Fullstack.SAXS.Infrastructure.IO;
using Fullstack.SAXS.Infrastructure.Kafka;
using Fullstack.SAXS.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fullstack.SAXS.Infrastructure
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
                .AddSingleton<IMessageHandler<TMessage>, THandler>()
                .AddHostedService<KafkaConsumer<TMessage>>();
        }
    }
}
