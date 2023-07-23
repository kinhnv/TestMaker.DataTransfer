using DataTransfer.Api.Configurations;
using DataTransfer.Api.KafkaConfigSetups.KafkaConsumerConfigSetups.KafkaUserQuestionConsumerConfigSetup;
using DataTransfer.Api.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TestmakerTestservice.Dbo.UserQuestions;
using i3rothers.TestServiceClient;

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

            var userQuestionsDataTransferClient = new UserQuestionsDataTransferClient(_gatewayConfiguration.Url, httpClient);

            if (value.After != null)
            {
                var userQuestion = await userQuestionsDataTransferClient.GetUserQuestionAsync(new Guid(value.After.UserId), new Guid(value.After.QuestionId));

                await httpClient.PostAsJsonAsync($"api/Event/DataTransfer/UserQuestions", userQuestion.Data);
                _logger.LogInformation("Create {UserQuestions}", JsonConvert.SerializeObject(userQuestion.Data));
            }
            else if (value.Before != null)
            {
                await httpClient.DeleteAsync($"api/Event/DataTransfer/UserQuestions?userId={value.Before.UserId}&questionId={value.Before.QuestionId}");
                _logger.LogInformation("Delete {QuestionId} - {UserId}", value.Before.QuestionId, value.Before.UserId);
            }
        }
    }
}
