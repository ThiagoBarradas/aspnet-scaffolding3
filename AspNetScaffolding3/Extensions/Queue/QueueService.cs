using AspNetScaffolding.Extensions.Configuration;
using AspNetScaffolding.Extensions.Queue.Interfaces;
using CQRS.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PackUtils;
using PackUtils.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspNetScaffolding.Extensions.Queue
{
    public static class QueueService
    {
        public static void SetupQueue(this IServiceCollection services)
        {
            if (Api.QueueSettings?.Enabled == true)
            {
                services.AddSingleton<QueueSettings>(Api.QueueSettings);
                services.AddSingleton<IQueueClient, QueueClient>();
                services.AddSingleton<IQueueProcessor>(provider =>
                {
                    var queueSettings = provider.GetService<QueueSettings>();
                    var queueClient = provider.GetService<IQueueClient>();
                    return new QueueProcessor(queueClient, queueSettings);
                });
            }
        }
    }
}