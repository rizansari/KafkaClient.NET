using Confluent.Kafka;
using KafkaClient.NET.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace KafkaClient.NET.Abstractions
{
    public abstract class KafkaConsumerAbstract<TKey, TValue>
    {
        protected IKafkaService _kafkaService;
        protected ConsumerOptions _options;
        protected KafkaOptions _kafkaOptions;

        protected Thread[] _th = null;
        protected ManualResetEvent _exit = null;
        protected CancellationTokenSource _source = new CancellationTokenSource();
        protected CancellationToken _token;
        protected int _threadCount;

        protected Action<ConsumeResult<TKey, TValue>> OnMessageReceived;

        public void StartConsumer(Action<ConsumeResult<TKey, TValue>> action)
        {
            _exit = new ManualResetEvent(false);
            _token = _source.Token;

            // start thread
            _th = new Thread[_threadCount];

            OnMessageReceived = action;

            for (int i = 0; i < _threadCount; i++)
            {
                _th[i] = new Thread(Run);
                _th[i].Name = string.Format("[{0}:{1}]", GetType(), i);
                _th[i].IsBackground = true;
                _th[i].Start();
            }
        }

        public void StopConsumer()
        {
            _source.Cancel();
            _exit.Set();
        }

        protected abstract void Run();
    }
}
