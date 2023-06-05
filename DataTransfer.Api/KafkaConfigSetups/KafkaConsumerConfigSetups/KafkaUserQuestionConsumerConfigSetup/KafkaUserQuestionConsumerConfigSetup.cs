using DataTransfer.Api.Configurations.KafkaConsumerConfigurations;
using DataTransfer.Api.Configurations;
using DataTransfer.Api.KafkaConfigSetups.KafkaConsumerConfigSetups.KafkaQuestionConsumerConfigSetup;
using Microsoft.Extensions.Options;

namespace DataTransfer.Api.KafkaConfigSetups.KafkaConsumerConfigSetups.KafkaUserQuestionConsumerConfigSetup
{
    public class KafkaUserQuestionConsumerConfigSetup : IConfigureOptions<KafkaUserQuestionConsumerConfig>
    {
        private readonly KafkaConfiguration _kafkaConfiguration;
        private readonly KafkaUserQuestionsConsumerConfiguration _kafkaQuestionsConsumerConfiguration;

        public KafkaUserQuestionConsumerConfigSetup(
            IOptions<KafkaConfiguration> kafkaConfigurationOptions,
            IOptions<KafkaUserQuestionsConsumerConfiguration> kafkaQuestionsConsumerConfigurationOptions)
        {
            _kafkaConfiguration = kafkaConfigurationOptions.Value;
            _kafkaQuestionsConsumerConfiguration = kafkaQuestionsConsumerConfigurationOptions.Value;
        }
        public void Configure(KafkaUserQuestionConsumerConfig options)
        {
            options.BootstrapServers = _kafkaConfiguration.BootstrapServers;
            options.AutoOffsetReset = _kafkaConfiguration.AutoOffsetReset;
            options.EnableAutoCommit = _kafkaConfiguration.EnableAutoCommit;

            options.GroupId = _kafkaQuestionsConsumerConfiguration.GroupId;
            options.Topic = _kafkaQuestionsConsumerConfiguration.Topic;
        }
    }
}
