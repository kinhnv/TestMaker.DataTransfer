using Confluent.Kafka;

namespace DataTransfer.Api.KafkaConfigSetups.KafkaConsumerConfigSetups
{
    public class KafkaConsumerConfig : ConsumerConfig
    {
        public string Topic { get; set; } = null!;
    }
}
