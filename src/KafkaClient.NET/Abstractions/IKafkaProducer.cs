using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KafkaClient.NET.Abstractions
{
    public interface IKafkaProducer<TKey, TValue>
    {
        Task<DeliveryResult<TKey, TValue>> ProduceAsync(Message<TKey, TValue> message);
    }
}
