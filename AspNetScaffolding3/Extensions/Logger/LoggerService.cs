using AspNetScaffolding.Extensions.AccountId;
using AspNetScaffolding.Extensions.RequestKey;
using AspNetScaffolding.Extensions.TimeElapsed;
using AspNetSerilog;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Builder;
using System.Collections.Generic;

namespace AspNetScaffolding.Extensions.Logger
{
    public static class LoggerServiceExtension
    {
        public static void SetupSerilog(
            this IServiceCollection services,
            string domain,
            string application,
            LoggerSettings settings,
            IEnumerable<string> ignoredRoutes)
        {
            var loggerBuilder = new LoggerBuilder();

            Log.Logger = loggerBuilder
                .UseSuggestedSetting(domain, application)
                .SetupSeq(settings?.SeqOptions)
                .SetupSplunk(settings?.SplunkOptions)
                .BuildLogger();

            if (settings?.DebugEnabled ?? false)
            {
                loggerBuilder.EnableDebug();
            }

            var config = new SerilogConfiguration
            {
                InformationTitle = settings?.TitlePrefix + CommunicationLogger.DefaultInformationTitle,
                ErrorTitle = settings?.TitlePrefix + CommunicationLogger.DefaultErrorTitle,
                Blacklist = settings?.JsonBlacklist,
                RequestKeyProperty = RequestKeyServiceExtension.RequestKeyHeaderName,
                AccountIdProperty = AccountIdServiceExtension.AccountIdHeaderName,
                TimeElapsedProperty = TimeElapsedServiceExtension.TimeElapsedHeaderName,
                IgnoredRoutes = ignoredRoutes
            };

            services.SetupSerilog(config);
        }
    }
}
