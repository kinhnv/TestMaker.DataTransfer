using DataTransfer.Api.KafkaConsumers;

namespace DataTransfer.Api.HostedServices
{
    public class UserQuestionsHostedService : IHostedService
    {
        private readonly IUserQuestionsKafkaConsumer _userQuestionsKafkaConsumer;

        public UserQuestionsHostedService(IUserQuestionsKafkaConsumer userQuestionsKafkaConsumer)
        {
            _userQuestionsKafkaConsumer = userQuestionsKafkaConsumer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _userQuestionsKafkaConsumer.InvokeAsync(cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
