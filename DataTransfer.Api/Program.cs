using DataTransfer.Api.Configurations;
using DataTransfer.Api.Configurations.KafkaConsumerConfigurations;
using DataTransfer.Api.HostedServices;
using DataTransfer.Api.KafkaConfigSetups;
using DataTransfer.Api.KafkaConfigSetups.KafkaConsumerConfigSetups.KafkaQuestionConsumerConfigSetup;
using DataTransfer.Api.KafkaConfigSetups.KafkaConsumerConfigSetups.KafkaUserQuestionConsumerConfigSetup;
using DataTransfer.Api.KafkaConsumers;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOptions<GatewayConfiguration>().Bind(builder.Configuration.GetSection("Gateway"));
builder.Services.AddOptions<KafkaConfiguration>().Bind(builder.Configuration.GetSection("Kafka"));
builder.Services.AddOptions<SchemaRegistryConfiguration>().Bind(builder.Configuration.GetSection("SchemaRegistry"));

builder.Services.AddOptions<KafkaQuestionsConsumerConfiguration>().Bind(builder.Configuration.GetSection("KafkaQuestionsConsumer"));
builder.Services.AddOptions<KafkaUserQuestionsConsumerConfiguration>().Bind(builder.Configuration.GetSection("KafkaUserQuestionsConsumer"));

builder.Services.ConfigureOptions<SchemaRegistryConfigSetup>();

builder.Services.ConfigureOptions<KafkaQuestionConsumerConfigSetup>();
builder.Services.ConfigureOptions<KafkaUserQuestionConsumerConfigSetup>();

builder.Services.AddTransient<IQuestionsKafkaConsumer, QuestionsKafkaConsumer>();
builder.Services.AddTransient<IUserQuestionsKafkaConsumer, UserQuestionsKafkaConsumer>();

builder.Services.AddHostedService<QuestionsHostedService>();
builder.Services.AddHostedService<UserQuestionsHostedService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
