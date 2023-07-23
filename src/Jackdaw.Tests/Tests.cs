using Confluent.Kafka;
using Xunit;

namespace Confluent;

public class Tests
{
    [Fact]
    public void Test()
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };

        var producer = new ProducerBuilder<string, string>(producerConfig)
            .Build();

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            GroupId = "my-group",
        };

        var consumer = new ConsumerBuilder<string, string>(consumerConfig)
            .SetLogHandler((_, log) => Console.WriteLine(log.Message))
            .Build();

        producer.Produce("my-topic", new Message<string, string> { Key = "key", Value = "value" });

        consumer.Subscribe("my-topic");
        var result = consumer.Consume();
    }
}
