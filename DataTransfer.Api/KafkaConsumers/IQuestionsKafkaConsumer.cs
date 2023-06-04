using DataTransfer.Api.KafkaConfigSetups.KafkaConsumerConfigSetups.KafkaQuestionConsumerConfigSetup;
using TestmakerTestservice.Dbo.Questions;

namespace DataTransfer.Api.KafkaConsumers
{
    public interface IQuestionsKafkaConsumer : IKafkaConsumer<KafkaQuestionConsumerConfig, Key, Envelope>
    {
    }
}
