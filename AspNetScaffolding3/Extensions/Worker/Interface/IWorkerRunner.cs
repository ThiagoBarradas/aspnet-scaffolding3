using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.Worker.Interface
{
    public interface IWorkerRunner
    {
        void Start();

        void Stop();
    }
}
