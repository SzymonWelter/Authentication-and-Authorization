using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Backend
{
    public class Startup
    {
        private readonly TokenValidationParameters _tokenValidadtionParameters;
        private readonly SymmetricSecurityKey _signingKey;
        public IConfigurationRoot Configuration { get; }


        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            _signingKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(Configuration.GetSection("TokenAuthentication: SecretKey").Value));
            //TODO Add section TokenAuthentication in appsetting.json

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = _tokenValidadtionParameters;
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting(routes =>
            {
                routes.MapGrpcService<AuthenticationService>();
            });
        }
    }
}