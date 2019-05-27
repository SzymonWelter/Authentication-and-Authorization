using backend.TokenServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Backend
{
    public class TokenProvider
    {
        private Dictionary<string, string> Tokens = new Dictionary<string, string>();
        public TokenStatus CheckToken(string token, string key, Grpc.Core.ServerCallContext context)
        {
            if (!Tokens.ContainsValue(token))
                return TokenStatus.Invalid;
            var decoded = EncryptDecrypt(token,key);

            var crc = new Crc();
            var ip = IPAddress.Parse("192.168.8.102").GetAddressBytes();//context.Host
            if (!crc.Valid(decoded.ToArray(), ip))
                return TokenStatus.Invalid;
            var data = new TokenData(decoded)
            {
                IP = IPAddress.Parse("192.168.8.102")////context.Host
            };
            return data.IsValid(context) ? TokenStatus.Valid : TokenStatus.OutOfDate;
        }

        //TODO Improve Encryptind/Decrypting method
        private byte [] EncryptDecrypt(string data, string key)
        {
            byte[] keyToByte = Encoding.ASCII.GetBytes(key);
            byte[] token = Encoding.GetEncoding("ISO-8859-1").GetBytes(data);
            return EncryptDecrypt(token, keyToByte);
        }
        private string EncryptDecrypt(byte [] data, string key)
        {
            byte[] keyToByte = Encoding.ASCII.GetBytes(key);
            byte [] result = EncryptDecrypt(data, keyToByte);
            return Encoding.GetEncoding("ISO-8859-1").GetString(result);
            
        }
        private byte[] EncryptDecrypt(byte [] data, byte [] key)
        {
            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < key.Length; j++)
                    data[i] = (byte)(data[i] ^ key[j]);
            }
            return data;
        }

        public string GenerateToken(TokenData tokenData, Grpc.Core.ServerCallContext context, string key)
        {
            var message = tokenData.CreateToken();
            var ip = IPAddress.Parse("192.168.8.102").GetAddressBytes(); //context.Host
            var crc = new Crc();
            var tokencrc = crc.Encode(message, ip);
            var token = EncryptDecrypt(tokencrc, key);
            Tokens.Add("192.168.8.102", token);//context.Host
            return token;           
        }

        public enum TokenStatus {
            Invalid,
            OutOfDate,
            Valid
        }
    }
}