using PackUtils;
using Serilog;
using Serilog.Context;
using System;
using System.Diagnostics;
using RK = AspNetScaffolding.Extensions.RequestKey;

namespace AspNetScaffolding.Extensions.Logger
{
    public class SimpleLogger : ISimpleLogger
    {
        public RK.RequestKey RequestKey { get; set; }

        public SimpleLogger(RK.RequestKey requestKey)
        {
            this.RequestKey = requestKey;
        }

        public void Error(string controller, string operation, string message, Exception exception, object additionalData = null)
        {
            StaticSimpleLogger.Error(controller, operation, message, exception, this.RequestKey.Value, additionalData);
        }

        public void Warning(string controller, string operation, string message, Exception exception, object additionalData = null)
        {
            StaticSimpleLogger.Warning(controller, operation, message, exception, this.RequestKey.Value, additionalData);
        }

        public void Fatal(string controller, string operation, string message, Exception exception, object additionalData = null)
        {
            StaticSimpleLogger.Fatal(controller, operation, message, exception, this.RequestKey.Value, additionalData);
        }

        public void Info(string controller, string operation, string message, object additionalData = null)
        {
            StaticSimpleLogger.Info(controller, operation, message, this.RequestKey.Value, additionalData);
        }

        public void Verbose(string controller, string operation, string message, object additionalData = null)
        {
            StaticSimpleLogger.Verbose(controller, operation, message, this.RequestKey.Value, additionalData);
        }

        public void Debug(string controller, string operation, string message, object additionalData = null)
        {
            StaticSimpleLogger.Debug(controller, operation, message, this.RequestKey.Value, additionalData);
        }

        public void Debug(string message, object additionalData = null)
        {
            StaticSimpleLogger.Debug(message, this.RequestKey.Value, additionalData);
        }

        public void Info(string message, object additionalData = null)
        {
            StaticSimpleLogger.Info(message, this.RequestKey.Value, additionalData);
        }

        public void Verbose(string message, object additionalData = null)
        {
            StaticSimpleLogger.Verbose(message, this.RequestKey.Value, additionalData);
        }

        public void Warning(string message, Exception exception = null, object additionalData = null)
        {
            StaticSimpleLogger.Warning(message, exception, this.RequestKey.Value, additionalData);
        }

        public void Error(string message, Exception exception = null, object additionalData = null)
        {
            StaticSimpleLogger.Error(message, exception, this.RequestKey.Value, additionalData);
        }

        public void Fatal(string message, Exception exception = null, object additionalData = null)
        {
            StaticSimpleLogger.Fatal(message, exception, this.RequestKey.Value, additionalData);
        }
    }
}
