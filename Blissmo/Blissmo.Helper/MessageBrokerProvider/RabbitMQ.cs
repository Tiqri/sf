using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blissmo.Helper.MessageBrokerProvider
{
    public class RabbitMQ : IMessageBroker
    {
        private IConnection _connection;
        private IModel _channel;

        public async Task ReceiveMessageAsync(BrokerConnection connection, Action<string> actionEvent)
        {
            var factory = new ConnectionFactory()
            {
                HostName = connection.EndPoint,
                Port = connection.Port,
                UserName = connection.UserName,
                Password = connection.Password
            };

            this._connection = factory.CreateConnection();
            this._channel = _connection.CreateModel();

            _channel.QueueBind(
                    queue: connection.QueueName,
                    exchange: "blissmodirectexchange",
                    routingKey: connection.QueueName,
                    arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                actionEvent(message);
            };

            consumer.ConsumerCancelled += (sender, ea) => Dispose();

            _channel.BasicConsume(
                queue: connection.QueueName,
                noAck: true,
                consumer: consumer
            );
        }

        public async Task SendMessageAsync<T>(BrokerConnection connection, T command) where T : class
        {
            var message = JsonConvert.SerializeObject(command);

            var factory = new ConnectionFactory()
            {
                HostName = connection.EndPoint,
                Port = connection.Port,
                UserName = connection.UserName,
                Password = connection.Password
            };

            using (var factoryCon = factory.CreateConnection())
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
                    queue: connection.QueueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                channel.QueueBind(connection.QueueName, "blissmodirectexchange", connection.QueueName);

                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(
                    exchange: "blissmodirectexchange",
                    routingKey: connection.QueueName,
                    basicProperties: null,
                    body: body);
            }
        }
        
        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();

            if (_channel != null)
                _channel.Dispose();
        }
    }
}
