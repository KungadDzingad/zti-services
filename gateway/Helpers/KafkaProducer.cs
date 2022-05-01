using Gateway.Helpers.Interfaces;
using Gateway.Models;
using Confluent.Kafka;
using System.Net;
using Newtonsoft.Json;

namespace Gateway.Helpers;

public class KafkaProducer : IKafkaProducer
{
    private readonly ProducerConfig _config;
    private readonly string _topic;
    public KafkaProducer()
    {
        _config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092",
            ClientId = Dns.GetHostName(),
        };
        _topic = "test123";
    }
    public async Task SendUserLoginOccurenceMessage(int userId, CancellationToken cancellationToken)
    {
        LogonEvent message = new LogonEvent
        {
            UserId = userId,
            Time = DateTime.UtcNow
        };
        string messageString = JsonConvert.SerializeObject(message);
        using (var producer = new ProducerBuilder<Null, string>(_config).Build())
        {
            await producer.ProduceAsync(_topic, new Message<Null, string> { Value = messageString}, cancellationToken);
        }
    }
}
