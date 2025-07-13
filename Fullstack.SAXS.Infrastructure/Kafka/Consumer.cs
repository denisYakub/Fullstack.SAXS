using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fullstack.SAXS.Infrastructure.Kafka
{
    public class Consumer : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ILogger<Consumer> _logger;


        public Consumer(IConfiguration config, ILogger<Consumer> logger)
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = config["Kafka:BootstrapServers"],
                GroupId = "saxs-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            _consumer.Subscribe("systems.created");

            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var cr = _consumer.Consume(stoppingToken);
                    _logger.LogInformation("[Kafka] Message: {Message}", cr.Message.Value);
                }
            }, stoppingToken);
        }

        public override void Dispose()
        {
            _consumer.Close();
            base.Dispose();
        }
    }
}
