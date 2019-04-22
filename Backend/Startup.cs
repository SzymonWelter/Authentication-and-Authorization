using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class Startup
    {
        private readonly TokenValidationParameters _tokenValidadtionParameters;
        private readonly TokenProviderOptions _tokenProviderOptions;
        private readonly SymmetricSecurityKey _signingKey;
        public IConfigurationRoot Configuration { get; }


        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            _signingKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(Configuration.GetSection("TokenAuthentication:SecretKey").Value));
            _tokenValidadtionParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,
                ValidateIssuer = true,
                ValidIssuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
                ValidateAudience = true,
                ValidAudience = Configuration.GetSection("TokenAuthentication:Audience").Value,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            _tokenProviderOptions = new TokenProviderOptions
            {
                Path = Configuration.GetSection("TokenAuthentication:TokenPath").Value,
                Audience = Configuration.GetSection("TokenAuthentication:Audience").Value,
                Issuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
                SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256),
                IdentityResolver = GetIdentity
            };
            
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc(options =>
            {
                options.EnableDetailedErrors = true;
            });
            ConfigureAuth(services);
            // TODO DBContext
        }

        public void ConfigureAuth(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = _tokenValidadtionParameters;
                })
                .AddCookie(options =>
                {
                    options.Cookie.Name = Configuration.GetSection("TokenAuthentication:CookieName").Value;
                    options.TicketDataFormat = new CustomJwtDataFormat(
                        SecurityAlgorithms.HmacSha256,
                        _tokenValidadtionParameters);
                });
                
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(_tokenProviderOptions));
            app.UseAuthentication();
            app.UseRouting(routes =>
            {
                routes.MapGrpcService<AuthenticationService>();
            });
        }

        private Task<ClaimsIdentity> GetIdentity(string  username,string password, UsersContext usersContext)
        {
            throw new NotImplementedException();
        }
    }
}