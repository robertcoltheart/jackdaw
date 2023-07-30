using Jackdaw;

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
    GroupId = "my-group"
};

var consumer = new ConsumerBuilder<string, string>(consumerConfig)
    .Build();

consumer.Subscribe("my-topic");

//await producer.ProduceAsync("my-topic", new Message<string, string> { Key = "key", Value = "value" });

var result = consumer.Consume();

Console.ReadKey();
