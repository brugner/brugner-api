using System;
using Brugner.API.Core.Contracts.Services;

namespace Brugner.API.Core.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void UpdateDatabase(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using var scope = scopeFactory.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetService<IDbManagerService>();

            dbInitializer?.Migrate();

            if (env.IsDevelopment())
            {
                dbInitializer?.Seed();
            }
        }
    }
}

