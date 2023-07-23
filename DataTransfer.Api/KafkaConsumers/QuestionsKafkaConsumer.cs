using DataTransfer.Api.Configurations;
using DataTransfer.Api.KafkaConfigSetups.KafkaConsumerConfigSetups.KafkaQuestionConsumerConfigSetup;
using DataTransfer.Api.Models;
using i3rothers.TestServiceClient;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TestmakerTestservice.Dbo.Questions;

namespace DataTransfer.Api.KafkaConsumers
{
    public class QuestionsKafkaConsumer : KafkaConsumer<KafkaQuestionConsumerConfig, Key, Envelope>, IQuestionsKafkaConsumer
    {
        private readonly GatewayConfiguration _gatewayConfiguration;
        private readonly ILogger<QuestionsKafkaConsumer> _logger;

        public QuestionsKafkaConsumer(
            IOptions<KafkaQuestionConsumerConfig> kafkaConsumerConfigOptions,
            IOptions<GatewayConfiguration> gatewayConfigurationOptions,
            ILogger<QuestionsKafkaConsumer> logger) : base(kafkaConsumerConfigOptions, logger)
        {
            _gatewayConfiguration = gatewayConfigurationOptions.Value;
            _logger = logger;
        }

        public override async Task HandleAsync(Key key, Envelope value)
        {
            if (value == null)
                return;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_gatewayConfiguration.Url)
            };

            var questionsDataTransferClient = new QuestionsDataTransferClient(_gatewayConfiguration.Url, httpClient);

            if (value.After != null)
            {
                var question = await questionsDataTransferClient.GetQuestionAsync(new Guid(value.After.QuestionId));

                await httpClient.PostAsJsonAsync($"api/Event/DataTransfer/Questions", question.Data);
                _logger.LogInformation("Create {Questions}", JsonConvert.SerializeObject(question.Data));
            }
            else if (value.Before != null)
            {
                await httpClient.DeleteAsync($"api/Event/DataTransfer/Questions?questionId={value.Before.QuestionId}");
                _logger.LogInformation("Delete {Questions}", value.Before.QuestionId);
            }
        }
    }
}
