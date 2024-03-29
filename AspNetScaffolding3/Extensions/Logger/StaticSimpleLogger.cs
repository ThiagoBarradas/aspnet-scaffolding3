﻿using PackUtils;
using Serilog;
using Serilog.Context;
using System;
using System.Diagnostics;

namespace AspNetScaffolding.Extensions.Logger
{
    public static class StaticSimpleLogger
    {
        public static string Version { get; set; } = "1.0.0";

        public static string Environment { get; set; } = "Development";

        public static string DefaultMessage => "[{Application}] [{Controller}] - [{Message}]";

        public static void UpdateVersion(string version)
        {
            Version = version;
        }

        public static void UpdateEnvironment(string environment)
        {
            Environment = environment;
        }

        public static void Error(string controller, string operation, string message, Exception exception, string requestKey = null, object additionalData = null)
        {
            SetProperties(controller, operation, message, exception, requestKey, additionalData);

            Log.Error(exception, DefaultMessage);
        }

        public static void Warning(string controller, string operation, string message, Exception exception, string requestKey = null, object additionalData = null)
        {
            SetProperties(controller, operation, message, exception, requestKey, additionalData);

            Log.Warning(exception, DefaultMessage);
        }

        public static void Fatal(string controller, string operation, string message, Exception exception, string requestKey = null, object additionalData = null)
        {
            SetProperties(controller, operation, message, exception, requestKey, additionalData);

            Log.Fatal(exception, DefaultMessage);
        }

        public static void Info(string controller, string operation, string message, string requestKey = null, object additionalData = null)
        {
            SetProperties(controller, operation, message, null, requestKey, additionalData);

            Log.Information(DefaultMessage);
        }

        public static void Verbose(string controller, string operation, string message, string requestKey = null, object additionalData = null)
        {
            SetProperties(controller, operation, message, null, requestKey, additionalData);

            Log.Verbose(DefaultMessage);
        }

        public static void Debug(string controller, string operation, string message, string requestKey = null, object additionalData = null)
        {
            SetProperties(controller, operation, message, null, requestKey, additionalData);

            Log.Debug(DefaultMessage);
        }

        private static void SetProperties(string controller, string operation, string message, Exception exception, string requestKey = null, object additionalData = null)
        {
            LogContext.PushProperty("Controller", controller);
            LogContext.PushProperty("Operation", operation);
            LogContext.PushProperty("Message", message);
            LogContext.PushProperty("ErrorException", exception);
            LogContext.PushProperty("RequestKey", requestKey);
            LogContext.PushProperty("ErrorMessage", exception?.Message);
            LogContext.PushProperty("Version", Version);
            LogContext.PushProperty("Environment", Environment);

            if (additionalData != null)
            {
                LogContext.PushProperty("AdditionalData", additionalData.Serialize().DeserializeAsObject());
            }
        }

        public static void Debug(string message, string requestKey = null, object additionalData = null)
        {
            var (methodName, className) = GetOrigin();
            Debug(className, methodName, message, requestKey, additionalData);
        }

        public static void Info(string message, string requestKey = null, object additionalData = null)
        {
            var (methodName, className) = GetOrigin();
            Info(className, methodName, message, requestKey, additionalData);
        }

        public static void Verbose(string message, string requestKey = null, object additionalData = null)
        {
            var (methodName, className) = GetOrigin();
            Verbose(className, methodName, message, requestKey, additionalData);
        }

        public static void Warning(string message, Exception exception = null, string requestKey = null, object additionalData = null)
        {
            var (methodName, className) = GetOrigin();
            Warning(className, methodName, message, exception, requestKey, additionalData);
        }

        public static void Error(string message, Exception exception = null, string requestKey = null, object additionalData = null)
        {
            var (methodName, className) = GetOrigin();
            Error(className, methodName, message, exception, requestKey, additionalData);
        }

        public static void Fatal(string message, Exception exception = null, string requestKey = null, object additionalData = null)
        {
            var (methodName, className) = GetOrigin();
            Fatal(className, methodName, message, exception, requestKey, additionalData);
        }

        public static (string _method, string _class) GetOrigin()
        {
            var methodInfo = new StackTrace().GetFrame(2).GetMethod();
            var className = methodInfo.ReflectedType.Name;

            return (methodInfo.Name, className);
        }
    }
}
