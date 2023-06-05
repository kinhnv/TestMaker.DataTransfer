using DataTransfer.Api.Configurations;
using DataTransfer.Api.KafkaConfigSetups.KafkaConsumerConfigSetups.KafkaUserQuestionConsumerConfigSetup;
using DataTransfer.Api.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TestmakerTestservice.Dbo.UserQuestions;

namespace DataTransfer.Api.KafkaConsumers
{
    public class UserQuestionsKafkaConsumer : KafkaConsumer<KafkaUserQuestionConsumerConfig, Key, Envelope>, IUserQuestionsKafkaConsumer
    {
        private readonly GatewayConfiguration _gatewayConfiguration;
        private readonly ILogger<UserQuestionsKafkaConsumer> _logger;

        public UserQuestionsKafkaConsumer(
            IOptions<KafkaUserQuestionConsumerConfig> kafkaConsumerConfigOptions,
            IOptions<GatewayConfiguration> gatewayConfigurationOptions,
            ILogger<UserQuestionsKafkaConsumer> logger) : base(kafkaConsumerConfigOptions, logger)
        {
            _gatewayConfiguration = gatewayConfigurationOptions.Value;
            _logger = logger;
        }

        public async override Task HandleAsync(Key key, Envelope value)
        {
            if (value == null)
                return;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_gatewayConfiguration.Url)
            };

            if (value.After != null)
            {
                var res = await httpClient.GetAsync($"api/Test/DataTransfer/UserQuestions?userId={value.After.UserId}&questionId={value.After.QuestionId}");

                if (res.IsSuccessStatusCode)
                {
                    var userQuestion = await res.Content.ReadFromJsonAsync<ServiceResult<UserQuestionForDataTransfer>>();

                    if (userQuestion?.Code == 200 && userQuestion?.Data != null)
                    {
                        await httpClient.PostAsJsonAsync($"api/Event/DataTransfer/UserQuestions", userQuestion.Data);
                        _logger.LogInformation("Create {UserQuestions}", userQuestion.Data);
                    }
                }
            }
            else if (value.Before != null)
            {
                await httpClient.DeleteAsync($"api/Event/DataTransfer/UserQuestions?userId={value.Before.UserId}&questionId={value.Before.QuestionId}");
                _logger.LogInformation("Delete {QuestionId} - {UserId}", value.Before.QuestionId, value.Before.UserId);
            }
        }
    }
}
