namespace DataTransfer.Api.Configurations.KafkaConsumerConfigurations
{
    public class KafkaConsumerConfiguration
    {
        public string Topic { get; set; } = null!;

        public string GroupId { get; set; } = null!;
    }
}
