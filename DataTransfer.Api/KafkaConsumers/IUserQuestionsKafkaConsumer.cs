using DataTransfer.Api.KafkaConfigSetups.KafkaConsumerConfigSetups.KafkaQuestionConsumerConfigSetup;
using TestmakerTestservice.Dbo.UserQuestions;

namespace DataTransfer.Api.KafkaConsumers
{
    public interface IUserQuestionsKafkaConsumer : IKafkaConsumer<KafkaQuestionConsumerConfig, Key, Envelope>
    {
    }
}
