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

        public override Task<AuthResponse> SignIn(SignInUserData user, Grpc.Core.ServerCallContext context)
        {
            
            // TODO
            /*
            ** Check if user with provided data exists in database
            ** if not return unauthenticated
            ** generate token
            ** Return token and 200 status
            */
            return Task.FromResult(new AuthResponse{Status = 200, Token = "Token"});
        } 

        public override Task<AuthResponse> SignUp(SignUpUserData user, Grpc.Core.ServerCallContext context)
        {
            // TODO

            /*
            ** Save new user in database
            ** Generate token
            ** Return token and 200 status
            */
            return Task.FromResult(new AuthResponse { Status = 200, Token = "Token" });
        }
    }
}