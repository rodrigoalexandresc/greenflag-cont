namespace GreenFlag.Commons.Messaging
{
    public interface IMessageProducer
    {
        Task ProduceAsync<T>(T json, string topicName);
    }
}
