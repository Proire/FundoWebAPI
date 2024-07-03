using Confluent.Kafka.Admin;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserRLL.Utilities
{
    public class KafkaAdminService
    {
        private readonly IConfiguration _configuration;

        public KafkaAdminService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task CreateTopicAsync()
        {
            var adminConfig = new AdminClientConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"]
            };

            using var adminClient = new AdminClientBuilder(adminConfig).Build();

            var topicName = _configuration["Kafka:Topic"];
            var numPartitions = int.Parse(_configuration["Kafka:Partitions"]);
            var replicationFactor = 1; // For simplicity, using 1 replication factor

            try
            {
                var topicSpecification = new TopicSpecification
                {
                    Name = topicName,
                    NumPartitions = numPartitions,
                    ReplicationFactor = (short)replicationFactor
                };

                await adminClient.CreateTopicsAsync(new List<TopicSpecification> { topicSpecification });
                Console.WriteLine($"Topic {topicName} created successfully.");
            }
            catch (CreateTopicsException e)
            {
                Console.WriteLine($"An error occurred creating topic {topicName}: {e.Results[0].Error.Reason}");
            }
        }
    }
}