using DataTransfer.Api.KafkaConsumers;

namespace DataTransfer.Api.HostedServices
{
    public class QuestionsHostedService : IHostedService
    {
        private readonly IQuestionsKafkaConsumer _questionsKafkaConsumer;

        public QuestionsHostedService(IQuestionsKafkaConsumer questionsKafkaConsumer)
        {
            _questionsKafkaConsumer = questionsKafkaConsumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _questionsKafkaConsumer.InvokeAsync(cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
