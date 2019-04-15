using System;
using System.Threading.Tasks;

namespace Backend
{
    public class AuthenticationService : Authentication.AuthenticationBase
    {
        IServiceProvider _services;

        public AuthenticationService(IServiceProvider services)
        {
            _services = services;
        }

        public override Task<AuthResponse> Authenticate(User user, Grpc.Core.ServerCallContext context)
        {
            return Task.FromResult(new AuthResponse{Status = 200, ExtraInfo = "Ok"});
        } 
    }
}