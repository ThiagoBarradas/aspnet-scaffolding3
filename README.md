<!-- [![Build status](https://ci.appveyor.com/api/projects/status/9jkiyh848g2djqkn/branch/master?svg=true)](https://ci.appveyor.com/project/ThiagoBarradas/aspnet-scaffolding/branch/master)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/a833f89548944fad8405aa6c9599edd4)](https://www.codacy.com/app/ThiagoBarradas/aspnet-scaffolding?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=ThiagoBarradas/aspnet-scaffolding&amp;utm_campaign=Badge_Grade) -->
[![NuGet Downloads](https://img.shields.io/nuget/dt/AspNetScaffolding3.svg)](https://www.nuget.org/packages/AspNetScaffolding3/)
[![NuGet Version](https://img.shields.io/nuget/v/AspNetScaffolding3.svg)](https://www.nuget.org/packages/AspNetScaffolding3/)

# AspNetScaffolding3

Build web api fast and easily with aspnet core > 3.1

## Install via NuGet

```
PM> Install-Package AspNetScaffolding3
```

# Create An Application

Basic steps:

- Creates a new empty web api project with aspnet core 3.1;
- Edit your `Program.cs` like example;
- Edit your `Startup.cs` like example;
- Create your `appsettings.json` and per environment, like `appsettings.Staging.json` (Environment is obtained by `ASPNETCORE_ENVIRONMENT`)
- Create your controllers inherit from `BaseController` from AspNetScaffolding;
- FluentValidation is automatic configured, just implements validator and use `this.Validate(obj);` in your action;
- This project uses `WebApi.Models` and can handler `ApiResponse` and `ApiException` like `NotFoundException`, `BadRequestException`, `UnauthorizedException`, etc;
- This project includes restsharp autolog for serilog with current RequestKey;
- To use AutoMapper you only need to create your mapping inheriting from Profile;

```c#
// Program.cs

public class Program
{
    public static void Main(string[] args)
    {
        var config = new ApiBasicConfiguration
        {
            ApiName = "My AspNet Scaffolding",
            ApiPort = 8700,
            EnvironmentVariablesPrefix = "Prefix_",
            ConfigureHealthcheck = Startup.ConfigureHealthcheck,
            ConfigureServices = Startup.ConfigureServices,
            Configure = Startup.Configure,
            AutoRegisterAssemblies = new Assembly[] 
                { Assembly.GetExecutingAssembly() }
        };

        Api.Run(config);
    }
}
```

```c#
// Startup.cs

public static class Startup
{
    public static void ConfigureHealthcheck(IHealthChecksBuilder builder, IServiceProvider provider)
    {
        // add health check configuration
        builder.AddMongoDb("mongodb://localhost:27017");
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        // add services
        services.AddSingleton<ISomething, Something>();
    }

    public static void Configure(IApplicationBuilder app)
    {
        // customize your app
        app.UseAuthentication();
    }
}

```

App Settings
```json
// appsettings.{environment}.json or appsettings.json

{
  "ApiSettings": {
    "AppUrl": "http://localhost:8700",
    "JsonSerializer": "Snakecase",
    "PathPrefix": "myapp/{version}",
    "UseStaticFiles": true,
    "StaticFilesPath": "assets",
    "Domain": "MyDomain",
    "Application": "MyApp",
    "Version": "v1",
    "BuildVersion": "1.0.0",
    "UseOriginalEnumValue": false,
    "SupportedCultures": [ "pt-BR", "es-ES", "en-US" ],
    "RequestKeyProperty": "RequestKey",
    "AccountIdProperty": "AccountId",
    "TimezoneHeader": "Timezone",
    "TimezoneDefault": "America/Sao_Paulo",
    "TimeElapsedProperty": "X-Internal-Time",
    "JsonFieldSelectorProperty": "fields"
  },
  "HealthcheckSettings": {
    "Enabled": true,
    "Path": "healthcheck",
    "LogEnabled": false
  },
  "ShutdownSettings": {
    "ShutdownTimeoutIsSeconds" : 30,
    "Enabled" : true,
    "Redirect" : false
  },
  "DbSettings": {
    "ConnectionString": "mongodb://user:pass@localhost:27017/DatabaseName",
    "Name": "DatabaseName"
  },
   "CacheSettings": {
    "Enabled": true,
    "UseRedis": true,
    "UseLocker": true,
    "TimeoutInMs": 1000,
    "Ssl": false,
    "Password": "RedisAuth",
    "Host": "localhost",
    "Port": 6379,
    "LockerPrefix": "app-locker-",
    "LockerTtlInSeconds": 100,
    "LockerDb": 0,  
    "CachePrefix": "app-cache-",
    "CacheTtlInSeconds": 900,
    "CacheDb": 0
  },
  "IpRateLimiting": {
    "Enabled" : true,
    "EnableEndpointRateLimiting": false,
    "StackBlockedRequests": false,
    "RealIpHeader": "X-Real-IP",
    "ClientIdHeader": "X-ClientId",
    "HttpStatusCode": 429,
    "IpWhitelist": [],
    "EndpointWhitelist": [],
    "ClientWhitelist": [],
    "GeneralRules": [
      {
        "Endpoint": "*",
        "Period": "1m",
        "Limit": 5
      },
      {
        "Endpoint": "*",
        "Period": "1h",
        "Limit": 1000
      }
    ],
    "QuotaExceededResponse": {
      "Content": "{{ \"message\": \"Quota exceeded. Maximum allowed: {0} per {1}. Please try again in {2} second(s).\" }}",
      "ContentType": "application/json",
      "StatusCode": 429
    }
  },
  "LogSettings": {
    "IgnoredRoutes": [],
    "DebugEnabled": true,
    "TitlePrefix": "[{Application}] ",
    "JsonBlacklistRequest":  [ "*password", "*card.number", "*creditcardnumber", "*cvv" ],
    "JsonBlacklistResponse": [ "*password", "*card.number", "*creditcardnumber", "*cvv" ],
    "SeqOptions": {
      "Enabled": true,
      "MinimumLevel": "Verbose",
      "Url": "http://localhost:5341",
      "ApiKey": "XXXX"
    },
    "NewRelicOptions": {
      "Enabled": false,
      "AppName": "Verbose",
      "LicenseKey": "XXXX"
    },
    "SplunkOptions": {
      "Enabled": false,
      "MinimumLevel": "Verbose",
      "Url": "http://localhost:8088/services/collector",
      "Token": "XXXX",
      "Index": "my.index",
      "Application": "MyApp :Ds",
      "ProcessName": "Domain.App",
      "Company": "MyCompany",
      "ProductVersion": "1.0.0",
      "SourceType": "_json"
    }
  },
  "DocsSettings": {
    "Enabled": true,
    "Title": "MyApp API Reference",
    "AuthorName": "Thiago Barradas",
    "AuthorEmail": "th.barradas@gmail.com",
    "PathToReadme": "DOCS.md",
    "IgnoredEnums": [ "none", "undefined" ]
  }
}

```

## How can I contribute?

Please, refer to [CONTRIBUTING](.github/CONTRIBUTING.md)

## Found something strange or need a new feature?

Open a new Issue following our issue template [ISSUE_TEMPLATE](.github/ISSUE_TEMPLATE.md)

## Changelog

See in [nuget version history](https://www.nuget.org/packages/AspNetScaffolding3)

## Did you like it? Please, make a donate :)

if you liked this project, please make a contribution and help to keep this and other initiatives, send me some Satochis.

BTC Wallet: `1G535x1rYdMo9CNdTGK3eG6XJddBHdaqfX`

![1G535x1rYdMo9CNdTGK3eG6XJddBHdaqfX](https://i.imgur.com/mN7ueoE.png)
