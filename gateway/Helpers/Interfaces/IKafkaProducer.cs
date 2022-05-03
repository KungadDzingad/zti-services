namespace Gateway.Helpers.Interfaces;

public interface IKafkaProducer
{
    Task SendUserLoginOccurenceMessage(int userId, CancellationToken cancellationToken);
}