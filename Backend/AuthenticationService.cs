using System.Threading.Tasks;

namespace Backend
{
    public class AuthenticationService : Authentication.AuthenticationBase
    {
        public override Task<AuthResponse> Authenticate(User user, Grpc.Core.ServerCallContext context)
        {
            return Task.FromResult(new AuthResponse{Status = 200, ExtraInfo = "Ok"});
        } 
    }
}