namespace RoofEstimation.BLL.Services.QueueService;

public interface IMessageProducerService
{
    void SendMessage<T>(T message);
}