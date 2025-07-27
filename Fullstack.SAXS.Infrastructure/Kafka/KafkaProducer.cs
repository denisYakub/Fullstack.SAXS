using System.Text.Json;
using Confluent.Kafka;
using Fullstack.SAXS.Application.Contracts;

namespace Fullstack.SAXS.Infrastructure.Kafka
{
    public class KafkaProducer<TMessage> : IProducer<TMessage>
    {
        private readonly IProducer<string, TMessage> producer;
        private readonly string topic;

        public KafkaProducer(IConnectionStrService connectionService)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = connectionService.KafkaUriPath().ToString(),
            };

            producer = new ProducerBuilder<string,  TMessage>(config)
                .SetValueSerializer(new KafkaJsonSerializer<TMessage>())
                .Build();

            topic = "system-create";
        }

        public Task ProduceAsync(TMessage message, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
