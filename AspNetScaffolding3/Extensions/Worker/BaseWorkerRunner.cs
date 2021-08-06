using AspNetScaffolding.Extensions.Logger;
using AspNetScaffolding.Extensions.Queue.Interfaces;
using AspNetScaffolding.Extensions.Worker.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.Worker
{
    public abstract class BaseWorkerRunner : IWorkerRunner
    {
        public BaseWorkerRunner(IQueueProcessor queueProcessor)
        {
            this.QueueProcessor = queueProcessor;
        }

        public IQueueProcessor QueueProcessor { get; set; }

        public Thread CurrentThread { get; set; }

        public abstract Task<bool> ExecuteAsync(string message, int retryCount, ulong deliveryTag);

        public void Start()
        {
            Task.Delay(1000).ContinueWith((state) =>
            {
                this.CurrentThread.Start();
                SimpleLogger.Info("WorkerRunner", nameof(Start), "Worker started!");
            });
        }

        public void Stop()
        {
            SimpleLogger.Info("WorkerRunner", nameof(Stop), "Worker stopped!");
            Thread.Sleep(2000);
            this.CurrentThread.Abort();
        }

        protected void InitFunction(Func<string, int, ulong, Task<bool>> func)
        {
            this.CurrentThread = new Thread(() =>
            {
                while (!this.QueueProcessor.ExecuteConsumer(func)) { }
            });
        }
    }

    public class DemoWorkerRunner : BaseWorkerRunner
    {
        public DemoWorkerRunner(IQueueProcessor queueProcessor) : base(queueProcessor)
        {
            this.QueueProcessor = queueProcessor;
            this.InitFunction(ExecuteAsync);
        }

        public override async Task<bool> ExecuteAsync(string message, int retryCount, ulong deliveryTag)
        {
            await Task.Delay(1);

            Console.WriteLine($"Message: {message}");

            return true;
        }
    }
}
