# Kafka Client library for .NET Core

Library Version: v1.0.0

## Installation

```powershell
Install-Package KafkaClient.NET
```

## Usage

KafkaClient.NET is a simple library to Produce and Consume easily in .NET Core applications. It is a wrapper on official Confluent.Kafka client library. It supports simple objects and Protocolbuffer objects.

### Setup DI
```
var serviceProvider = new ServiceCollection()
        .AddLogging(loggingBuilder =>
        {
            loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        })
        .AddKafka(options =>
        {
            options.BootstrapServers = "10.1.1.1:9092"; // replace with yours
        })
                .BuildServiceProvider();
        .BuildServiceProvider();
```

### Get Kafka Service instance
```
var kafka = serviceProvider.GetRequiredService<IKafkaService>();
```

### Simple Producer Example
Create producer object
```
var p1 = kafka.CreateProducer<Null, string>(options =>
  {
      options.TopicName = "test-topic-01";
  });
```
Produce message
```
var dr = p1.ProduceAsync(new Message<Null, string> { Value = "test" }).Result;
Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
```

### Simple Consumer Example
Create consumer object
```
var c1 = kafka.CreateConsumer<Null, string>(options =>
  {
      options.TopicName = "test-topic-02";
      options.GroupId = "1";
      options.ThreadCount = 8;
  });
```
Start consuming
```
c1.StartConsumer(MessageReceived);
```
Stop consuming
```
c1.StopConsumer();
```

### Producer Example with protobuf
Create producer object
```
var p2 = kafka.CreateProducerProtobuf<string, User>(options =>
  {
      options.TopicName = "test-topic-03";
      options.SchemaRegistryUrl = "10.1.1.1:8081"; // replace with yours
  });
```
Produce message
```
User user = new User { Name = "Imran Khan", FavoriteColor = "green", FavoriteNumber = 100 };
var dr = p2.ProduceAsync(new Message<string, User> { Key = user.Name, Value = user }).Result;
Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
```

### Consumer Example with protobuf
Create consumer object
```
var c2 = kafka.CreateConsumerProtobuf<string, User>(options =>
{
    options.TopicName = "test-topic-03";
    options.GroupId = "30";
    options.ThreadCount = 1;
    options.AutoOffsetReset = AutoOffsetReset.Earliest;
});
```
Start consuming
```
c2.StartConsumer(MessageReceived);
```
Stop consuming
```
c2.StopConsumer();
```
## License

This library licensed under the MIT license.
