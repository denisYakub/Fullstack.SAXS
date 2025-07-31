using System.Threading.Tasks;
using Confluent.Kafka;
using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Infrastructure.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fullstack.SAXS.Infrastructure.Kafka
{
    public class KafkaConsumer<TMessage> : BackgroundService
    {
        private readonly string _topic;
        private readonly IConsumer<string, TMessage> _consumer;
        private readonly IMessageHandler<TMessage> _handler;
        private readonly ILogger<KafkaConsumer<TMessage>> _logger;

        public KafkaConsumer(
            IOptions<KafkaOptions> options, 
            IMessageHandler<TMessage> handler, 
            ILogger<KafkaConsumer<TMessage>> logger
        )
        {
            var confiq = new ConsumerConfig
            {
                BootstrapServers = options.Value.Uri,
                GroupId = options.Value.GroupId
            };

            _topic = options.Value.Topic;

            _consumer = new ConsumerBuilder<string, TMessage>(confiq)
                .SetValueDeserializer(new KafkaJsonDeserializer<TMessage>())
                .Build();

            _handler = handler;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => ConsumeAsync(stoppingToken));
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Close();

            return base.StopAsync(cancellationToken);
        }

        private async Task ConsumeAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_topic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = _consumer.Consume(stoppingToken);

                    await _handler
                        .HandleAsync(result.Message.Value, stoppingToken)
                        .ConfigureAwait(false);
                }
            }
            catch (ConsumeException)
            {
                LogMessage(_logger, LogLevel.Error, "Consumer exception.");
                throw;
            }
            catch (OperationCanceledException)
            {
                LogMessage(_logger, LogLevel.Error, "Consumer is canceled.");
                throw;
            }
        }

        private static void LogMessage(ILogger logger, LogLevel level, string message)
        {
            var eventId = level switch
            {
                LogLevel.Information => new EventId(1001, "InfoMessage"),
                LogLevel.Warning => new EventId(1002, "WarningMessage"),
                LogLevel.Error => new EventId(1003, "ErrorMessage"),
                _ => new EventId(1000, "GenericMessage")
            };

            logger.Log(level, eventId, message);
        }
    }
}
