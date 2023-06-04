using DataTransfer.Api.KafkaConfigSetups.KafkaConsumerConfigSetups;
using Google.Protobuf;

namespace DataTransfer.Api.KafkaConsumers
{
    public interface IKafkaConsumer<TKafkaConsumerConfig, TKey, TValue> 
        where TKafkaConsumerConfig : KafkaConsumerConfig 
        where TKey : class, IMessage<TKey>, new()
        where TValue : class, IMessage<TValue>, new()
    {
        Task InvokeAsync(CancellationToken cancellationToken);

        Task HandleAsync(TKey key, TValue value);
    }
}
