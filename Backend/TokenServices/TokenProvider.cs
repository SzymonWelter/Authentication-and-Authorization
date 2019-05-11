using System.Collections.Generic;

namespace Backend
{
    public class TokenProvider
    {
        private Dictionary<string, string> Tokens = new Dictionary<string, string>();

        public bool ValidateToken(string token, Grpc.Core.ServerCallContext context)
        {
            if (!Tokens.ContainsValue(token))
                return false;
            var encoded = EncodeToken(token);
            return encoded.IsValid(context);               
        }

        public string GetToken(TokenData tokenData)
        {

        }

        public TokenData EncodeToken(string token)
        {

        }



    }
}