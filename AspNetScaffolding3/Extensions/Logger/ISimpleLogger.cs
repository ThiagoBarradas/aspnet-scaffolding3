using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.Logger
{
    public interface ISimpleLogger
    {
        void Error(string controller, string operation, string message, Exception exception, object additionalData = null);

        void Warning(string controller, string operation, string message, Exception exception, object additionalData = null);
        
        void Fatal(string controller, string operation, string message, Exception exception, object additionalData = null);

        void Info(string controller, string operation, string message, object additionalData = null);

        void Verbose(string controller, string operation, string message, object additionalData = null);

        void Debug(string controller, string operation, string message, object additionalData = null);

        void Error(string message, Exception exception, object additionalData = null);

        void Warning(string message, Exception exception, object additionalData = null);

        void Fatal(string message, Exception exception, object additionalData = null);

        void Info(string message, object additionalData = null);

        void Verbose(string message, object additionalData = null);

        void Debug(string message, object additionalData = null);
    }
}
