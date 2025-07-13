using Confluent.Kafka;
using Fullstack.SAXS.Application.Contracts;
using Microsoft.Extensions.Configuration;

namespace Fullstack.SAXS.Infrastructure.Kafka
{
    public class Producer : IEventPublisher
    {
        private readonly IProducer<Null, string> _producer;

        public Producer(IConfiguration configuration)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task PublishAsync(string topic, string message)
        {
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
        }
    }
}
