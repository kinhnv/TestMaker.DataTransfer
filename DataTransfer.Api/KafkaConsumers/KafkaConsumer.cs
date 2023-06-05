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
        private readonly ILogger<KafkaConsumer<TKafkaConsumerConfig, TKey, TValue>> _logger;

        public KafkaConsumer(IOptions<TKafkaConsumerConfig> kafkaConsumerConfigOptions, ILogger<KafkaConsumer<TKafkaConsumerConfig, TKey, TValue>> logger)
        {
            _kafkaConsumerConfig = kafkaConsumerConfigOptions.Value;
            _logger = logger;
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
                catch (Exception e)
                {
                    if (consumer != null)
                    {
                        consumer.Close();
                    }
                    _logger.LogInformation("Restart after 5000 because error: {Error}", e.Message);
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
