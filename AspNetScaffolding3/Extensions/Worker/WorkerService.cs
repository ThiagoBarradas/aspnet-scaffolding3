using AspNetScaffolding.Extensions.Configuration;
using AspNetScaffolding.Extensions.Queue.Interfaces;
using AspNetScaffolding.Extensions.Worker;
using AspNetScaffolding.Extensions.Worker.Interface;
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
    public static class WorkerService
    {
        public static void SetupWorker<T>(this IServiceCollection services) where T : BaseWorkerRunner
        {
            if (Api.WorkerSettings?.Enabled == true)
            {
                services.AddSingleton<IWorkerRunner, T>();

                var provider = services.BuildServiceProvider();

                provider.GetService<IWorkerRunner>().Start();
                Console.CancelKeyPress += (sender, _event) =>
                {
                    provider.GetService<IWorkerRunner>().Stop();
                };
            }
        }
    }
}