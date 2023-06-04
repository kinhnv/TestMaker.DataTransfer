using Confluent.SchemaRegistry;
using DataTransfer.Api.Configurations;
using Microsoft.Extensions.Options;

namespace DataTransfer.Api.KafkaConfigSetups
{
    public class SchemaRegistryConfigSetup : IConfigureOptions<SchemaRegistryConfig>
    {
        private readonly SchemaRegistryConfiguration _schemaRegistryConfiguration;

        public SchemaRegistryConfigSetup(IOptions<SchemaRegistryConfiguration> schemaRegistryConfigurationOptions)
        {
            _schemaRegistryConfiguration = schemaRegistryConfigurationOptions.Value;
        }

        public void Configure(SchemaRegistryConfig options)
        {
            options.Url = _schemaRegistryConfiguration.Url;
        }
    }
}
