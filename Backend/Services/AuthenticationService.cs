using System;
using System.Net;
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
            
            var service = (TokenProvider)_services.GetService(typeof(TokenProvider));
            var tokenData = new TokenData() {
                Username = user.Username,
                Expiration = DateTime.Now.AddMinutes(5),
                IP = IPAddress.Parse("192.168.8.102"),//context.Host
                AuthVersion = 1
            };
            var token = service.GenerateToken(tokenData, context, "secretkey");

            return Task.FromResult(new AuthResponse{Status = 200, Token = token});
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
        public override Task<AuthResponse> AuthTest(SignUpUserData data, Grpc.Core.ServerCallContext context)
        {
            var service = (TokenProvider)_services.GetService(typeof(TokenProvider));
            var res = service.CheckToken(data.Password, "secretkey", context);
            return Task.FromResult(new AuthResponse { Status = res == TokenProvider.TokenStatus.Invalid ? 400 : 200, Token = data.Password });
        }
    }
}