using backend.TokenServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Backend
{
    public class TokenData
    {
        public DateTime Expiration { get; set; } //DateTime -> string -> long -> 64 bits (8 bytes)
        public string Username { get; set; } //string -> 320 bits (40 bytes)
        public int AuthVersion { get; set; } //int -> 8 bits (1 bytes)
        public IPAddress IP { get; set; } //string -> 32 bits (4 bytes)(IPv4) as CRC polynomial
                                          //TODO IP could be in 6th version -> 48 bits (6 bytes)(IPv6)      

        public TokenData() { }
        public TokenData(byte [] token)
        {
            var expiration = BitConverter.ToInt64(token, 0);
            Expiration = DateTime.ParseExact(expiration.ToString(), "yyyyMMddHHmmssfff",null);
            Username = Encoding.ASCII.GetString(token, 8, 40);
            AuthVersion = token[48];            
        }

        public byte [] CreateToken()
        {
            List<byte> bytes = new List<byte>();
            var datenum = long.Parse(Expiration.ToString("yyyyMMddHHmmssfff"));
            bytes.AddRange(BitConverter.GetBytes(datenum));
            bytes.AddRange(Encoding.ASCII.GetBytes(Username + new string((char)0,40-Username.Length)));
            bytes.Add((byte)AuthVersion);
            bytes.AddRange(IP.GetAddressBytes());
            return bytes.ToArray();
        }

        public bool IsValid(Grpc.Core.ServerCallContext context)
        {
            return DateTime.Compare(DateTime.Now, Expiration) > 0 &&
                    context.Host.Equals(IP); 
                    // && AuthVersion == ?; 
        }
    }
}