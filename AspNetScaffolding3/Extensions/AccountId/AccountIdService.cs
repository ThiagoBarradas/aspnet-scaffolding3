using Microsoft.Extensions.DependencyInjection;

namespace AspNetScaffolding.Extensions.AccountId
{
    public static class AccountIdServiceExtension
    {
        public static string AccountIdHeaderName = "AccountId";

        public static void SetupAccountId(this IServiceCollection services, string headerName = null)
        {
            if (string.IsNullOrWhiteSpace(headerName) == false)
            {
                AccountIdHeaderName = headerName;
            }

            services.AddScoped<AccountIdMiddleware>();
            services.AddScoped(obj => new AccountId());
        }
    }
}
