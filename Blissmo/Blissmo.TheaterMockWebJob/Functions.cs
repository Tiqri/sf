using System;
using System.IO;
using System.Linq;
using RabbitMQ.Client;
using WebJobs.Extensions.RabbitMQ;

namespace Blissmo.TheaterMockWebJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage(
            [RabbitQueueBinder(
                exchange: "blissmodirectexchange",
                routingKey: "",
                errorExchange: "",
                autoDelete: false,
                durable: false,
                execlusive: false)]
            [RabbitQueueTrigger("reservationqueue")]
                string message,
            TextWriter log
            )
        {
            log.WriteLine(message);

            var resultsArr = new string[] {
                "{ 'IsSuccess': 'False', 'Description': 'Seats are not available'}",
                "{ 'IsSuccess': 'True', 'Description': 'Successful'}"
            };

            var rng = new Random();
            var randomElement = resultsArr[rng.Next(resultsArr.Count())];

            //publish message
            var rabbitMq = new RabbitMq();
            var factory = new ConnectionFactory()
            {
                HostName = "52.170.16.118",
                Port = 5672,
                UserName = "user",
                Password = "eXile1234567"
            };
            rabbitMq.SendMessageAsync<string>(
                factory,
                "reservationresponse",
                randomElement);
        }
    }
}
