using DataTransfer.Api.Configurations;
using DataTransfer.Api.Configurations.KafkaConsumerConfigurations;
using Microsoft.Extensions.Options;

namespace DataTransfer.Api.KafkaConfigSetups.KafkaConsumerConfigSetups.KafkaQuestionConsumerConfigSetup
{
    public class KafkaQuestionConsumerConfigSetup : IConfigureOptions<KafkaQuestionConsumerConfig>
    {
        private readonly KafkaConfiguration _kafkaConfiguration;
        private readonly SchemaRegistryConfiguration _schemaRegistryConfiguration;
        private readonly KafkaQuestionsConsumerConfiguration _kafkaQuestionsConsumerConfiguration;

        public KafkaQuestionConsumerConfigSetup(
            IOptions<KafkaConfiguration> kafkaConfigurationOptions,
            IOptions<SchemaRegistryConfiguration> schemaRegistryConfigurationOptions,
            IOptions<KafkaQuestionsConsumerConfiguration> kafkaQuestionsConsumerConfigurationOptions)
        {
            _kafkaConfiguration = kafkaConfigurationOptions.Value;
            _schemaRegistryConfiguration = schemaRegistryConfigurationOptions.Value;
            _kafkaQuestionsConsumerConfiguration = kafkaQuestionsConsumerConfigurationOptions.Value;
        }
        public void Configure(KafkaQuestionConsumerConfig options)
        {
            options.BootstrapServers = _kafkaConfiguration.BootstrapServers;
            options.AutoOffsetReset = _kafkaConfiguration.AutoOffsetReset;
            options.EnableAutoCommit = _kafkaConfiguration.EnableAutoCommit;

            options.GroupId = _kafkaQuestionsConsumerConfiguration.GroupId;
            options.Topic = _kafkaQuestionsConsumerConfiguration.Topic;
        }
    }
}
