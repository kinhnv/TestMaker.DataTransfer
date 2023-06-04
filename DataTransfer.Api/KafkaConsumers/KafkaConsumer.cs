using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;
using DataTransfer.Api.KafkaConfigSetups.KafkaConsumerConfigSetups;
using Google.Protobuf;
using Microsoft.Extensions.Options;

namespace DataTransfer.Api.KafkaConsumers
{
    public abstract class KafkaConsumer<TKafkaConsumerConfig, TKey, TValue> : IKafkaConsumer<TKafkaConsumerConfig, TKey, TValue>
        where TKafkaConsumerConfig : KafkaConsumerConfig 
        where TKey : class, IMessage<TKey>, new() 
        where TValue : class, IMessage<TValue>, new()
    {
        private readonly TKafkaConsumerConfig _kafkaConsumerConfig;

        public KafkaConsumer(IOptions<TKafkaConsumerConfig> kafkaConsumerConfigOptions)
        {
            _kafkaConsumerConfig = kafkaConsumerConfigOptions.Value;
        }

        public abstract Task HandleAsync(TKey key, TValue value);

        public async Task InvokeAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_kafkaConsumerConfig.Topic))
            {
                return;
            }

            while (true)
            {
                IConsumer<TKey, TValue>? consumer = null;

                try
                {
                    consumer = SetupConsumer();

                    consumer.Subscribe(_kafkaConsumerConfig.Topic);

                    var message = consumer.Consume(cancellationToken);

                    await HandleAsync(message.Message.Key, message.Message.Value);

                    consumer.Commit();
                }
                catch (Exception)
                {
                    if (consumer != null)
                    {
                        consumer.Close();
                    }

                    Thread.Sleep(5000);

                    await InvokeAsync(cancellationToken);
                }
            }
        }

        private IConsumer<TKey, TValue> SetupConsumer()
        {
            return new ConsumerBuilder<TKey, TValue>(_kafkaConsumerConfig)
                .SetKeyDeserializer(new ProtobufDeserializer<TKey>().AsSyncOverAsync())
                .SetValueDeserializer(new ProtobufDeserializer<TValue>().AsSyncOverAsync())
                .Build();
        }
    }
}
