using Confluent.Kafka;
using LogonEvents.Models;
using LogonEvents.Services.Interfaces;
using Newtonsoft.Json;

namespace LogonEvents.BackgroundServices;

public class KafkaReader : IHostedService
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
            GroupId = "foo",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        _topic = "test123";
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _=Read(cancellationToken);
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

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
