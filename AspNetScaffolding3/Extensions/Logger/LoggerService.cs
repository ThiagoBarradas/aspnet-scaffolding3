﻿using AspNetScaffolding.Extensions.AccountId;
using AspNetScaffolding.Extensions.RequestKey;
using AspNetScaffolding.Extensions.TimeElapsed;
using AspNetScaffolding.Utilities;
using AspNetSerilog;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Builder;
using System;
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
            List<string> ignoredRoutes)
        {
            var loggerBuilder = new LoggerBuilder();

            if (settings?.NewRelicOptions?.Enabled == true && string.IsNullOrWhiteSpace(settings?.NewRelicOptions?.LicenseKey))
            {
                settings.NewRelicOptions.LicenseKey = Environment.GetEnvironmentVariable("NEW_RELIC_LICENSE_KEY");
            }

            Log.Logger = loggerBuilder
                .UseSuggestedSetting(domain, application)
                .SetupSeq(settings?.SeqOptions)
                .SetupSplunk(settings?.SplunkOptions)
                .SetupNewRelic(settings?.NewRelicOptions)
                .SetupDataDog(settings?.DataDogOptions)
                .DisableConsoleIfConsoleSinkIsEnabled(settings?.ConsoleOptions)
                .BuildConfiguration()
                .EnableStdOutput(settings?.ConsoleOptions)
                .CreateLogger();

            if (settings?.DebugEnabled ?? false)
            {
                loggerBuilder.EnableDebug();
            }

            var config = new SerilogConfiguration
            {
                Version = Api.ApiSettings.BuildVersion,
                InformationTitle = settings?.TitlePrefix + settings?.GetInformationTitle(),
                ErrorTitle = settings?.TitlePrefix + settings?.GetErrorTitle(),
                BlacklistRequest = settings?.GetJsonBlacklistRequest(),
                BlacklistResponse = settings?.JsonBlacklistResponse,
                HeaderBlacklist = settings?.HeaderBlacklist,
                HttpContextBlacklist = settings?.HttpContextBlacklist,
                QueryStringBlacklist = settings?.QueryStringBlacklist,
                RequestKeyProperty = RequestKeyServiceExtension.RequestKeyHeaderName,
                AccountIdProperty = AccountIdServiceExtension.AccountIdHeaderName,
                TimeElapsedProperty = TimeElapsedServiceExtension.TimeElapsedHeaderName,
                IgnoredRoutes = ignoredRoutes
            };

            StaticSimpleLogger.UpdateVersion(Api.ApiSettings.BuildVersion);
            StaticSimpleLogger.UpdateEnvironment(EnvironmentUtility.GetCurrentEnvironment());

            services.SetupSerilog(config);
            services.AddScoped<ISimpleLogger, SimpleLogger>();
        }
    }
}
