using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.TokenServices
{
    public class Crc
    {
        public byte [] Generate(byte [] token, long polynomial) //returns token+crc
        {
            var newToken = new List<byte>(token);
            newToken.AddRange(new byte[8]);
            token = newToken.ToArray();
            for (int i = 0; i < token.Length; i++)
            {
                Xor(token, i, BitConverter.GetBytes(polynomial));
            }
            throw new NotImplementedException();
        }

        private void Xor(byte [] token, int position, byte [] polynomial)
        {
            //left shift token position times and xor with polynomial
            throw new NotImplementedException();
        }

        public bool Valid(byte[] token, long polynomial)
        {
            var result = Generate(token, polynomial);
            return result.All(x => x == 0);
        }
    }
}
