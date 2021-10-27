using KafkaClient.NET.Abstractions;
using KafkaClient.NET.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaClient.NET.DependencyInjectionExtensions
{
    public static class KafkaExtensions
    {
        //public static IServiceCollection AddKafka(this IServiceCollection services)
        //{
        //    var temp = new KafkaOptions();
        //    services.AddSingleton<KafkaOptions>(temp);
        //    //services.TryAddSingleton<IQueueService, QueueService>();
        //    return AddKafka(services);
        //}

        public static IServiceCollection AddKafka(this IServiceCollection services, Action<KafkaOptions> options)
        {
            var temp = new KafkaOptions();
            options(temp);
            services.Configure<KafkaOptions>(opt => opt.BootstrapServers = temp.BootstrapServers);
            services.TryAddSingleton<IKafkaService, KafkaService>();
            return services;
        }
    }
}
