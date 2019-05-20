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

        private int position = 0;
        private int BytePosition => position / 8;
        private byte Mask => (byte)Math.Pow(2, 7 - (position % 8));
        public byte [] Encode() //returns token+crc
        {
            position = 0;
            var codeCreator = new List<byte>(Message); // Message = n bytes + 1 bit, 7 bit is already part of crc           
            codeCreator.AddRange(new byte[Divisor.Length - 1]); //Divisor.Length - 1 because 7 bits was added in line before
            var code = codeCreator.ToArray();
            var polynomial = new byte[code.Length];
            for(int i = 0; i < Divisor.Length; i++)
            {
                polynomial[i] = Divisor[i];
            }

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
            return position <= (Message.Length - 1) * 8; 
        }

        private byte [] Xor(byte [] data, byte [] polynomial)
        {
            for(int i = data.Length - 1; i >= BytePosition; i--)
            {
                data[i] = (byte)(data[i] ^ polynomial[i]);
            }
            return data;
        }


        public bool Valid(byte[] token, byte [] polynomial)
        {
            position = 0;
            Divisor = polynomial;
            Message = token.Take(token.Length-polynomial.Length+1).ToArray();
            polynomial = new byte[token.Length];
            for (int i = 0; i < Divisor.Length; i++)
            {
                polynomial[i] = Divisor[i];
            }
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
