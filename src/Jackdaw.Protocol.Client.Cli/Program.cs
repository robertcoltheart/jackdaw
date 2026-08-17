using Confluent.Kafka;

var producerConfig = new ProducerConfig
{
    BootstrapServers = "localhost:9092"
};

var consumerConfig = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    AutoOffsetReset = AutoOffsetReset.Earliest,
    GroupId = "my-group1"
};

using var producer = new ProducerBuilder<string, string>(producerConfig)
    .Build();

using var consumer = new ConsumerBuilder<string, string>(consumerConfig)
    .Build();

producer.Produce("my-topic", new Message<string, string>
{
    Key = "key",
    Value = "value"
});

consumer.Subscribe("my-topic");

var result = consumer.Consume();

Console.ReadKey();
