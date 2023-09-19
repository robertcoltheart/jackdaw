# Jackdaw

[![Docs](https://img.shields.io/badge/docs-wiki-blue.svg?style=for-the-badge)](https://github.com/robertcoltheart/jackdaw/wiki) [![NuGet](https://img.shields.io/nuget/v/Jackdaw?style=for-the-badge)](https://www.nuget.org/packages/Jackdaw) [![Discussions](https://img.shields.io/badge/DISCUSS-ON%20GITHUB-yellow?style=for-the-badge)](https://github.com/robertcoltheart/jackdaw/discussions) [![License](https://img.shields.io/github/license/robertcoltheart/jackdaw?style=for-the-badge)](https://github.com/robertcoltheart/jackdaw/blob/master/LICENSE)

A high-performance Kafka client written in managed C#.

_Jackdaw aims to be API-compatible with the official Confluent Kafka library, so that any migration to Jackdaw should be seamless, even down to the config settings used._

Jackdaw aims to be:
 - 🏃 High-performance with low memory usage
 - 🔧 Extensible for use cases such as AWS IAM authentication
 - 🤝 Cross-platform using fully managed code
 - ✅ API-compliant to the official Confluent Kafka library


## Usage
Install the package from NuGet with `dotnet add package Jackdaw`.

Create your producer and/or consumer with the correct settings, and publish/subscribe to the topics you need, as below:

```csharp
var producerConfig = new ProducerConfig
{
    BootstrapServers = "localhost:9092"
};

var consumerConfig = new ConsumerConfig
{
    BootstrapServers = "localhost:9092",
    GroupId = "my-group"
};

using var producer = new ProducerBuilder<string, string>(producerConfig)
    .Build();

using var consumer = new ConsumerBuilder<string, string>(consumerConfig)
    .Build();

producer.Produce("my-topic", new Message<string, string>
{
    Key = "key", Value = "value"
});

consumer.Subscribe("my-topic");

var result = consumer.Consume();
```

## Documentation
See the [wiki](https://github.com/robertcoltheart/jackdaw/wiki) for examples and help using Jackdaw.

## Get in touch
Discuss with us on [Discussions](https://github.com/robertcoltheart/jackdaw/discussions), or raise an [issue](https://github.com/robertcoltheart/jackdaw/issues).

[![Discussions](https://img.shields.io/badge/DISCUSS-ON%20GITHUB-yellow?style=for-the-badge)](https://github.com/robertcoltheart/jackdaw/discussions)

## Contributing
Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on how to contribute to this project.

## Acknowledgements
With much ❤️ to the [Confluent Kafka](https://github.com/confluentinc/confluent-kafka-dotnet) library.

## License
Jackdaw is released under the [Apache 2.0 License](LICENSE)
