

// using System.Text;
// using Newtonsoft.Json;
// using RabbitMQ.Client;
//
// namespace RoofEstimation.BLL.Services.QueueService;
//
// public class MessageProducerService : IEventBus, IDisposable
// {
//     public async Task SendMessage<T>(T message)
//     {
//         var factory = new ConnectionFactory() { HostName = "rabbit-server", Port = 5672, UserName="guest", Password="guest" };
//         var connection = await factory.CreateConnectionAsync();
//         using (var channel = await connection.CreateChannelAsync())
//         {
//
//             await channel.QueueDeclareAsync(queue: "orders",
//                 durable: false,
//                 exclusive: false,
//                 autoDelete: false,
//                 arguments: null);
//             var json = JsonConvert.SerializeObject(message);
//             var body = Encoding.UTF8.GetBytes(json);
//               
//             await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "orders", mandatory: true, body: body, basicProperties: null);
//         }
//     }
// }