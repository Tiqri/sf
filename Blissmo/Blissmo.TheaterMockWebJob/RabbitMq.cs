using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Blissmo.TheaterMockWebJob
{
    public class RabbitMq
    {
        public void SendMessageAsync<T>(ConnectionFactory connectionFactory, string queueName, T command) where T : class
        {
            var message = JsonConvert.SerializeObject(command);

            using (var factoryCon = connectionFactory.CreateConnection())
            using (var channel = factoryCon.CreateModel())
            {
                channel.ExchangeDeclare(
                    exchange: "blissmodirectexchange",
                    type: "direct",
                    durable: false,
                    autoDelete: false,
                    arguments: null
                );

                channel.QueueDeclare(
                    queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(
                    exchange: "blissmodirectexchange",
                    routingKey: queueName,
                    basicProperties: null,
                    body: body);
            }
        }
    }
}
