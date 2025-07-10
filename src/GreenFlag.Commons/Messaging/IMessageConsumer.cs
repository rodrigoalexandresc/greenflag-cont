namespace GreenFlag.Commons.Messaging
{
    public interface IMessageConsumer
    {
        Task Consume<T>(string topicName, Func<T, CancellationToken, Task> func, CancellationToken stoppingToken);
    }
}
