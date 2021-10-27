using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using KafkaClient.NET;
using KafkaClient.NET.Abstractions;
using KafkaClient.NET.DependencyInjectionExtensions;
using KafkaClient.NET.Options;
using Log4NetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace KafkaClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //setup DI
            var serviceProvider = new ServiceCollection()
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    loggingBuilder.AddLog4Net();
                })
                // add kafka
                .AddKafka(options =>
                {
                    options.BootstrapServers = "10.1.1.226:9092";
                })
                .BuildServiceProvider();
            
            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            // get QueueService
            var kafka = serviceProvider.GetRequiredService<IKafkaService>();

            //// create producer
            //var p1 = kafka.CreateProducer<Null, string>(options =>
            //{
            //    options.TopicName = "test-topic-01";
            //});

            //// send message
            //var dr = p1.ProduceAsync(new Message<Null, string> { Value = "test" }).Result;
            //Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");

            //// create consumer
            //var c1 = kafka.CreateConsumer<Null, string>(options =>
            //{
            //    options.TopicName = "test-topic-02";
            //    options.GroupId = "1";
            //    options.ThreadCount = 8;
            //});

            // start consumer
            //c1.StartConsumer(MessageReceived);
            //Console.WriteLine("Consumer Started");
            //Console.ReadLine();
            //c1.StopConsumer();

            // create producer using protobuf serialization

            //var p2 = kafka.CreateProducerProtobuf<string, User>(options =>
            //{
            //    options.TopicName = "test-topic-03";
            //    options.SchemaRegistryUrl = "http://10.1.1.226:8081";
            //});

            //// send message
            //User user = new User { Name = "Imran Khan", FavoriteColor = "green", FavoriteNumber = 100 };
            //var dr = p2.ProduceAsync(new Message<string, User> { Key = user.Name, Value = user }).Result;
            //Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");


            // create consumer using protobuf serialization
            var c2 = kafka.CreateConsumerProtobuf<string, User>(options =>
            {
                options.TopicName = "test-topic-03";
                options.GroupId = "30";
                options.ThreadCount = 1;
                options.AutoOffsetReset = AutoOffsetReset.Earliest;
            });

            // start consumer
            c2.StartConsumer(MessageReceived);
            Console.WriteLine("Consumer Started");
            Console.ReadLine();
            c2.StopConsumer();

            Console.ReadLine();
        }

        static void MessageReceived(ConsumeResult<Null, string> cr)
        {
            Console.WriteLine(cr.Message.Value);
        }

        static void MessageReceived(ConsumeResult<string, User> cr)
        {
            Console.WriteLine($"Name: {cr.Message.Value.Name}; Color: {cr.Message.Value.FavoriteColor}; Number: {cr.Message.Value.FavoriteNumber}");
        }
    }
}
