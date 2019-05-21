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
        public byte [] Divisor { get; set; } //4 bytes

        private int position = 0;
        private int BytePosition => position / 8;
        private byte Mask => (byte)Math.Pow(2, 7 - (position % 8));
        public byte [] Encode() //returns token+crc
        {
            position = 0;
            var codeCreator = new List<byte>(Message);        
            codeCreator.AddRange(new byte[Divisor.Length]);
            var code = codeCreator.ToArray();
            var polynomial = new byte[code.Length];
            for(int i = 0; i < Divisor.Length; i++)
            {
                polynomial[i] = Divisor[i];
            }
            RightShift(polynomial);
            polynomial[0] += 128; // first bit must be one

            ChangePosition(code, polynomial);
            while (IsNextStep())
            {                
                code = Xor(code, polynomial);// xor bit by bit               
                ChangePosition(code, polynomial);
            }
            for(int i = 1; i <= Divisor.Length; i++)
            {
                codeCreator[codeCreator.Count-i] += code[^i]; 
            }
            return codeCreator.ToArray();
        }

        private void ChangePosition(byte [] code, byte[] polynomial)
        {          
            while( BytePosition < code.Length && (Mask & code[BytePosition]) == 0)
            {
                RightShift(polynomial);
                position++;
            }
            
        }

        private void RightShift(byte[] polynomial)
        {
            for(int i = polynomial.Length-1; i > BytePosition; i--)
            {
                polynomial[i] = (byte)(polynomial[i] >> 1);
                polynomial[i] += (byte)(polynomial[i - 1] % 2 == 0 ? 0 : 128);               
            }
            polynomial[BytePosition] = (byte)(polynomial[BytePosition] >> 1); 
        }


        private bool IsNextStep()
        {
            return position < (Message.Length) * 8; 
        }

        private byte [] Xor(byte [] data, byte [] polynomial)
        {
            for(int i = data.Length - 1; i >= BytePosition; i--)
            {
                data[i] = (byte)(data[i] ^ polynomial[i]);
            }
            return data;
        }


        public bool Valid(byte[] token, byte [] polynomial) //first bit of token must be one
        {
            position = 0;
            Divisor = polynomial;
            Message = token.Take(token.Length-polynomial.Length).ToArray();
            polynomial = new byte[token.Length];
            for (int i = 0; i < Divisor.Length; i++)
            {
                polynomial[i] = Divisor[i];
            }
            RightShift(polynomial);
            polynomial[0] += 128; // first bit must be one
            ChangePosition(token, polynomial);
            while (IsNextStep())
            {
                token = Xor(token, polynomial);// xor bit by bit               
                ChangePosition(token, polynomial);
            }
            return token.All(x => x == 0);
        }
    }
}
