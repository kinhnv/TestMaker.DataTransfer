using DataTransfer.Api.Configurations;
using DataTransfer.Api.KafkaConfigSetups.KafkaConsumerConfigSetups.KafkaQuestionConsumerConfigSetup;
using DataTransfer.Api.Models;
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

            if (value.After != null)
            {
                var res = await httpClient.GetAsync($"api/Test/DataTransfer/Questions?questionId={value.After.QuestionId}");

                if (res.IsSuccessStatusCode)
                {
                    var question = await res.Content.ReadFromJsonAsync<ServiceResult<QuestionForDataTransfer>>();

                    if (question?.Code == 200 && question?.Data != null)
                    {
                        await httpClient.PostAsJsonAsync($"api/Event/DataTransfer/Questions", question.Data);
                        _logger.LogInformation("Create {Questions}", JsonConvert.SerializeObject(question.Data));
                    }
                }
            }
            else if (value.Before != null)
            {
                await httpClient.DeleteAsync($"api/Event/DataTransfer/Questions?questionId={value.Before.QuestionId}");
                _logger.LogInformation("Delete {Questions}", value.Before.QuestionId);
            }
        }
    }
}
