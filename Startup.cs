using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[assembly: FunctionsStartup(typeof(CrudAzureFunc.Startup))]

namespace CrudAzureFunc
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var keyValuatUrl = new Uri(Environment.GetEnvironmentVariable("KeyVaultUrl"));
            var secretClient = new SecretClient(keyValuatUrl, new DefaultAzureCredential());
            var cs = secretClient.GetSecret("sqlquerystring").Value.Value;

            // builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(cs));
            builder.Services.AddDbContext<AppDbContext>(options =>
                 options.UseSqlServer(cs, sqlServerOptionsAction: sqlOptions =>
                 {
                     sqlOptions.EnableRetryOnFailure(
                         maxRetryCount: 5,
                         maxRetryDelay: TimeSpan.FromSeconds(30),
                         errorNumbersToAdd: null);
                 }));
        }
    }
}
