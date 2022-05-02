using Confluent.Kafka;
using LogonEvents.Models;
using LogonEvents.Services.Interfaces;
using Newtonsoft.Json;

namespace LogonEvents.BackgroundServices;

public class KafkaReader : BackgroundService
{
    private readonly ConsumerConfig _config;
    private readonly string _topic;
    private readonly IServiceProvider _serviceProvider;
    public KafkaReader(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "logonEventsConsumer",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _topic = "logonEvents";
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Task.Run(() => Read(stoppingToken));
        return Task.CompletedTask;
    }

    public async Task Read(CancellationToken cancellationToken)
    {
        using (var consumer = new ConsumerBuilder<Ignore, string>(_config).Build())
        {
            consumer.Subscribe(_topic);

            while (!cancellationToken.IsCancellationRequested)
            {

                var consumeResult = consumer.Consume(cancellationToken);

                try
                {
                    LogonEvent? logonEvent = JsonConvert.DeserializeObject<LogonEvent>(consumeResult.Message.Value);
                    
                    if (logonEvent != null)
                    {
                        using (IServiceScope scope = _serviceProvider.CreateScope())
                        {
                            ILogonEventService logonEventService =
                                scope.ServiceProvider.GetRequiredService<ILogonEventService>();

                            await logonEventService.AddLogonEvent(logonEvent, cancellationToken);
                        }
                    }
                }
                catch (Exception)
                {}
            }

            consumer.Close();
        }
    }
}
