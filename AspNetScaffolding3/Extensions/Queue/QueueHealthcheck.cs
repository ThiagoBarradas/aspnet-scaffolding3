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
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;

namespace AspNetScaffolding.Extensions.Queue
{
    public static class QueueHealthcheck
    {
        public static void AddRabbitMqAutomatic(IHealthChecksBuilder builder, IServiceProvider provider)
        {
            var queueSettings = provider.GetService<QueueSettings>();
            var sslOptions = new SslOption
            {
                Enabled = true,
                AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateNotAvailable | SslPolicyErrors.RemoteCertificateChainErrors | SslPolicyErrors.RemoteCertificateNameMismatch
            };
            builder.AddRabbitMQ(queueSettings.QueueConnectionString, (queueSettings.UseSsl() ? sslOptions : null), "rabbitmq");
        }
    }
}