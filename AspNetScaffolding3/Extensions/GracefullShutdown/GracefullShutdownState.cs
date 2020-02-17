using System.Threading;

namespace AspNetScaffolding3.Extensions.GracefullShutdown
{
    public class GracefullShutdownState : IRequestsCountProvider
    {
        private long _requestsInProgress;
        public long RequestsInProgress => Volatile.Read(ref _requestsInProgress);

        private long _requestsProcessed;
        public long RequestsProcessed => Volatile.Read(ref _requestsProcessed);
        
        private bool _stopRequested;
        public bool StopRequested => Volatile.Read(ref _stopRequested);

        public void NotifyRequestStarted()
        {
            Interlocked.Increment(ref _requestsInProgress);
        }

        public void NotifyRequestFinished()
        {
            Interlocked.Decrement(ref _requestsInProgress);
            Interlocked.Increment(ref _requestsProcessed);
        }

        public void NotifyStopRequested()
        {
            Volatile.Write(ref _stopRequested, true);
        }
    }
}
