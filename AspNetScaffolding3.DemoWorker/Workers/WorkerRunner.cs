using System.Threading.Tasks;
using AspNetScaffolding.Extensions.Logger;
using AspNetScaffolding.Extensions.Queue.Interfaces;
using AspNetScaffolding.Extensions.RequestKey;
using AspNetScaffolding.Extensions.Worker;


namespace AspNetScaffolding3.DemoWorker.Workers
{
    public class WorkerRunner : BaseWorkerRunner
    {
        public WorkerRunner(IQueueProcessor queueProcessor) : base(queueProcessor)
        {
            this.InitFunction(ExecuteAsync);
        }

        public override async Task<bool> ExecuteAsync(string message, int retryCount, ulong deliveryTag, string requestKey)
        {
            var simpleLogger = new SimpleLogger(new RequestKey(requestKey));

            var teste = new
            {
                test = "1",
                test_1 = "123",
                password = "hellomyfriend",
                creditCard = "123456788911"
            };

            simpleLogger
                .Info(nameof(WorkerRunner), "CreateTransact", "Generate cryptogram", teste);

            return true;
        }
    }
}