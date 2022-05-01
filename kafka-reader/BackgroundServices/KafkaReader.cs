using Confluent.Kafka;
using LogonEvents.Services.Interfaces;

namespace LogonEvents.BackgroundServices;

public class KafkaReader : IHostedService, IDisposable
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
        using (var consumer = new ConsumerBuilder<Ignore, string>(_config).Build())
        {
            consumer.Subscribe(_topic);

            while (!cancellationToken.IsCancellationRequested)
            {

                var consumeResult = consumer.Consume(cancellationToken);
                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    ILogonEventService logonEventService =
                        scope.ServiceProvider.GetRequiredService<ILogonEventService>();

                    //await logonEventService.AddLogonEvent();
                }
                
            }

            consumer.Close();
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
    }
}
