using Confluent.Kafka;

namespace DataTransfer.Api.Configurations
{
    public class KafkaConfiguration
    {
        public string BootstrapServers { get; set; } = null!;

        public AutoOffsetReset AutoOffsetReset { get; set; }

        public bool EnableAutoCommit { get; set; }
    }
}
