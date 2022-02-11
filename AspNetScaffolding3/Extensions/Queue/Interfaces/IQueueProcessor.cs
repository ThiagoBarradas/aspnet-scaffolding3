using System;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.Queue.Interfaces
{
    public interface IQueueProcessor : IDisposable
    {
        bool ExecuteConsumer(Func<string, int, ulong, string, Task<bool>> func);

        void HandleFailedEvent(string message, int retryCount, ulong deliveryTag, string requestKey);

        void HandleSuccededEvent(ulong deliveryTag);
    }
}
