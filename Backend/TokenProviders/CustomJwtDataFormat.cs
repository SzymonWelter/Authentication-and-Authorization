﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
namespace Backend
{
    internal class CustomJwtDataFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly string _algorithm;
        private readonly TokenValidationParameters _validationParameters;

        public CustomJwtDataFormat(string alghorithm, TokenValidationParameters validationParameters)
        {
            _algorithm = alghorithm;
            _validationParameters = validationParameters;
        }

        public string Protect(AuthenticationTicket data)
        {
            throw new NotImplementedException();
        }

        public string Protect(AuthenticationTicket data, string purpose)
        {
            throw new NotImplementedException();
        }

        public AuthenticationTicket Unprotect(string protectedText)
            => Unprotect(protectedText, null);

        public AuthenticationTicket Unprotect(string protectedText, string purpose)
        {
            var handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = null;

            try
            {
                SecurityToken validToken = null;
                principal = handler.ValidateToken(protectedText, _validationParameters, out validToken);

                var validJwt = validToken as JwtSecurityToken;

                if (validJwt == null)
                {
                    throw new System.ArgumentException("Invalid JWT");
                }

                if (!validJwt.Header.Alg.Equals(_algorithm, StringComparison.Ordinal))
                {
                    throw new ArgumentException($"Algorithm must be '{_algorithm}'");
                }
            }
            catch (SecurityTokenValidationException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }
            return new AuthenticationTicket(principal, new AuthenticationProperties(), "Cookie");
        }
    }
}