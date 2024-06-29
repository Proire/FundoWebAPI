using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace UserRLL.Utilities
{
    public class RabitMQProducer(IConfiguration configuration) : IRabitMQProducer
    {
        private readonly string _hostName = configuration["RabbitMQ:HostName"] ?? throw new Exception("Not able to initialized HostName");
        private readonly string _queueName = configuration["RabbitMQ:QueueName"] ?? throw new Exception("Not able to initialized QueueName");
        public void SendMessage<T>(T message)
        {
            // Create a connection factory
            var factory = new ConnectionFactory { HostName = _hostName };

            // Establish a connection
            using var connection = factory.CreateConnection();

            // Create a channel
            using var channel = connection.CreateModel();

            // Declare a queue
            channel.QueueDeclare(queue: _queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var json = JsonConvert.SerializeObject(message);
            // Convert the message to a byte array
            var body = Encoding.UTF8.GetBytes(message.ToString() ?? throw new Exception("Message is null, please provide message"));

            // Publish the message to the queue
            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: _queueName,
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine($" [x] Sent {message}");
        }
    }
}
