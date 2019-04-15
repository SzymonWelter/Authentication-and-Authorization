using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Backend
{
    internal class TokenProviderOptions
    {
        public string Path { get; set; } = "/token";
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
        public Func<string, string, UsersContext, Task<ClaimsIdentity>> IdentityResolver { get; set; }
        public Func<Task<string>> NonceGenerator { get; set; } = () => Task.FromResult(Guid.NewGuid().ToString());
        public TimeSpan Expiration { get; internal set; } = TimeSpan.FromMinutes(5);
    }
}