using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.TokenServices
{
    public class Crc
    {
        public byte [] Message { get; set; }
        public byte [] Divisor { get; set; }
        public int CrcLength { get => Divisor.Length * 8 - 1; }


        public byte [] Encode() //returns token+crc
        {
            var codeCreator = new List<byte>(Message);            
            codeCreator.AddRange(new byte[CrcLength]);
            var code = codeCreator.ToArray();
            int i = 0;
            while (IsNextStep())
            {
                code = Xor(code,i++);// xor bit by bit
            }
            return code;
        }
        public byte[] Encode(byte[] divisor) //returns token+crc
        {
            Divisor = divisor;
            return Encode();
        }

        private bool IsNextStep()
        {
            
            
        }

        private byte [] Xor(byte [] data)
        {
            if (data.Length != Divisor.Length) {
                throw new ArgumentException("The length of both arrays must be the same");
            }
            for(int i = 0; i < data.Length; i++)
            {
                data[i] = (byte) (data[i] ^ Divisor[i]);
            }
            return data;        
        }

        private byte [] Xor(byte[] data, int offset){
            //TODO
            //1) left rotation
            //2) remove first bit
            //3) add new bit
            //4) xor with Divisor
            //5) return result 
            throw new NotImplementedException();
        }

        public bool Valid(byte[] token, byte [] polynomial)
        {
            var result = Encode(polynomial);
            return result.All(x => x == 0);
        }
    }
}
