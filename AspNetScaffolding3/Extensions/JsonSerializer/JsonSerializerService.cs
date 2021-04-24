﻿using CQRS.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PackUtils;
using PackUtils.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspNetScaffolding.Extensions.JsonSerializer
{
    public static class JsonSerializerService
    {
        public static JsonSerializerSettings JsonSerializerSettings { get; set; }

        public static Newtonsoft.Json.JsonSerializer JsonSerializer { get; set; }

        public static void ConfigureJsonSettings(
            this IMvcBuilder mvc,
            IServiceCollection services,
            JsonSerializerEnum jsonSerializerMode,
            string timezoneHeaderName,
            TimeZoneInfo defaultTimeZone)
        {
            CaseUtility.JsonSerializerMode = jsonSerializerMode;

            JsonSerializerSettings = null;
            JsonSerializer = null;

            if (Api.ApiSettings.UseOriginalEnumValue)
            {
                JsonUtility.DefaultConverters = new List<JsonConverter>
                {
                    new StringEnumConverter(new OriginalCaseNamingResolver()),
                    new IsoDateTimeConverter
                    {
                        DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.ffffff"
                    }
                };
            }

            switch (jsonSerializerMode)
            {
                case JsonSerializerEnum.Camelcase:
                    JsonSerializer = JsonUtility.CamelCaseJsonSerializer;
                    JsonSerializerSettings = JsonUtility.CamelCaseJsonSerializerSettings;
                    break;
                case JsonSerializerEnum.Lowercase:
                    JsonSerializer = JsonUtility.LowerCaseJsonSerializer;
                    JsonSerializerSettings = JsonUtility.LowerCaseJsonSerializerSettings;
                    break;
                case JsonSerializerEnum.Snakecase:
                    JsonSerializer = JsonUtility.SnakeCaseJsonSerializer;
                    JsonSerializerSettings = JsonUtility.SnakeCaseJsonSerializerSettings;
                    break;
                default:
                    break;
            }

            // for fluent validation in cqrs extensions
            PropertyName.Resolver = (propertyName) =>
            {
                var parts = propertyName.Split('.');
                return string.Join(".", parts.Select(r => r.ToCase(jsonSerializerMode.ToString())));
            };

            JsonConvert.DefaultSettings = () => JsonSerializerSettings;

            services.AddScoped((provider) => JsonSerializer);
            services.AddScoped((provider) => JsonSerializerSettings);

            DateTimeConverter.DefaultTimeZone = defaultTimeZone;
            mvc.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = JsonSerializerSettings.ContractResolver;
                options.SerializerSettings.Converters = JsonSerializerSettings.Converters;
                options.SerializerSettings.NullValueHandling = JsonSerializerSettings.NullValueHandling;
                options.SerializerSettings.Converters.Add(new DateTimeConverter(() =>
                {
                    var httpContextAccessor = services.BuildServiceProvider().GetService<IHttpContextAccessor>();
                    return DateTimeConverter.GetTimeZoneByAspNetHeader(httpContextAccessor, timezoneHeaderName);
                }));
            });
        }
    }
}
