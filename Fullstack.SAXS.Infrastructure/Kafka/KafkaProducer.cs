using Confluent.Kafka;
using Fullstack.SAXS.Application.Contracts;
using Fullstack.SAXS.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Fullstack.SAXS.Infrastructure.Kafka
{
    public class KafkaProducer<TMessage> : IProducer<TMessage>
    {
        private readonly IProducer<string, TMessage> _producer;
        private readonly string _topic;

        public KafkaProducer(IOptions<KafkaOptions> options)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = options.Value.Uri,
            };

            _producer = new ProducerBuilder<string,  TMessage>(config)
                .SetValueSerializer(new KafkaJsonSerializer<TMessage>())
                .Build();

            _topic = options.Value.Topic;
        }

        public async Task ProduceAsync(TMessage message, CancellationToken cancellationToken = default)
        {
            await _producer.ProduceAsync(_topic, new Message<string, TMessage>
            {
                Key = "uniq1",
                Value = message
            }, cancellationToken);
        }

        public void Dispose()
        {
            _producer.Dispose();
        }
    }
}
