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
            ConfigureMapper = Startup.AdditionalConfigureMapper,
            ConfigureHealthcheck = Startup.AdditionalConfigureHealthcheck,
            ConfigureServices = Startup.AdditionalConfigureServices,
            Configure = Startup.AdditionalConfigure,
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
    public static void AdditionalConfigureHealthcheck(IHealthChecksBuilder builder, IServiceProvider services)
    {
        // add health check configuration
        builder.AddMongoDb("mongodb://localhost:27017");
    }

    public static void AdditionalConfigureServices(IServiceCollection services)
    {
        // add services
        services.AddSingleton<ISomething, Something>();
    }

    public static void AdditionalConfigure(IApplicationBuilder app)
    {
        // customize your app
        app.UseAuthentication();
    }

    public static void AdditionalConfigureMapper(IMapperConfigurationExpression mapper)
    {
        // customize your mappers
        mapper.CreateMap<SomeModel, OtherModel>();
    }
}

```

App Settings
```json
// appsettings.{environment}.json or appsettings.json

{
  "ApiSettings": {
    "AppUrl": "http://localhost:5855",
    "JsonSerializer": "Snakecase",
    "PathPrefix": "myapp/{version}",
    "Domain": "MyDomain",
    "Application": "MyApp",
    "Version": "v1",
    "BuildVersion": "1.0.0",
    "SupportedCultures": [ "pt-BR", "es-ES", "en-US" ],
    "RequestKeyProperty": "RequestKey",
    "AccountIdProperty": "AccountId",
    "TimezoneHeader": "Timezone",
    "TimezoneDefault": "America/Sao_Paulo",
    "TimeElapsedProperty": "X-Internal-Time"
  },
  "HealthcheckSettings": {
    "Enabled" :  true,
    "Path": "healthcheck/some-token-if-needed",
    "LogEnabled" : false
  }
},
  "LogSettings": {
    "DebugEnabled": false,
    "TitlePrefix": "[{Application}] ",
    "JsonBlacklist": [ "*password", "*card.number", "*creditcardnumber", "*cvv" ],
    "SeqOptions": {
      "Enabled": true,
      "MinimumLevel": "Verbose",
      "Url": "http://localhost:5341",
      "ApiKey": "XXXX"
    },
    "SplunkOptions": {
      "Enabled": false,
      "MinimumLevel": "Verbose",
      "Url": "http://localhost:8088/services/collector",
      "Token": "XXXX",
      "Index": "my.index",
      "Application": "MyApp",
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
    "PathToReadme": "DOCS.md"
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
