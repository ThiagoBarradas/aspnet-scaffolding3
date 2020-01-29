using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.AccountId
{
    public class AccountIdMiddleware
    {
        private readonly RequestDelegate Next;

        public AccountIdMiddleware(RequestDelegate next)
        {
            this.Next = next;
        }

        public async Task Invoke(HttpContext context, AccountId accountId)
        {
            await this.Next(context);

            context.Items.Add(AccountIdServiceExtension.AccountIdHeaderName, accountId.Value);
        }
    }

    public static class AccountIdMiddlewareExtension
    {
        public static void UseAccountId(this IApplicationBuilder app)
        {
            app.UseMiddleware<AccountIdMiddleware>();
        }
    }
}
