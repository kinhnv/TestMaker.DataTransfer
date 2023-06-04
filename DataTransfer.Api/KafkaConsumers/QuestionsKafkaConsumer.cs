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

        public QuestionsKafkaConsumer(
            IOptions<KafkaQuestionConsumerConfig> kafkaConsumerConfigOptions,
            IOptions<GatewayConfiguration> gatewayConfigurationOptions) : base(kafkaConsumerConfigOptions)
        {
            _gatewayConfiguration = gatewayConfigurationOptions.Value;
        }

        public override async Task HandleAsync(Key key, Envelope value)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_gatewayConfiguration.Url)
            };

            if (value.After != null)
            {
                var res = await httpClient.GetAsync($"api/Test/DataTransfer/Questions?questionId={value.After.QuestionId}");

                if (res.IsSuccessStatusCode)
                {
                    var userQuestion = await res.Content.ReadFromJsonAsync<ServiceResult<QuestionForDataTransfer>>();

                    if (userQuestion?.Code == 200 && userQuestion?.Data != null)
                    {
                        await httpClient.PostAsJsonAsync($"api/Event/DataTransfer/Questions", userQuestion.Data);
                    }
                }
            }
            else if (value.Before != null)
            {
                await httpClient.DeleteAsync($"api/Event/DataTransfer/UserQuestions?questionId={value.Before.QuestionId}");
            }
        }
    }
}
