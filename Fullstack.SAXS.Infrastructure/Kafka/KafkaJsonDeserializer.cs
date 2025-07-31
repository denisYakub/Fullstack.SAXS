using System.Text.Json;
using Confluent.Kafka;

namespace Fullstack.SAXS.Infrastructure.Kafka
{
    internal class KafkaJsonDeserializer<TMessage> : IDeserializer<TMessage>
    {
        public TMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return JsonSerializer.Deserialize<TMessage>(data)!;
        }
    }
}
