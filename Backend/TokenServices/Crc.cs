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
        public byte [] Polynomial { get; set; } //4 bytes

        private int bitPosition = 0;
        private int BytePosition => bitPosition / 8;
        private byte Mask => (byte)Math.Pow(2, 7 - (bitPosition % 8));
        
        public byte [] Encode(byte[] message, byte[] polynomial) //returns token+crc
        {
            bitPosition = 0;
            Message = message;
            Polynomial = polynomial;
            var token = Message.Concat(new byte[Polynomial.Length]).ToArray();            
            var divisor = Polynomial.Concat(new byte[token.Length - Polynomial.Length]).ToArray(); ;
            var crcReminder = CrcReminder(token, divisor);
            return Message.Concat(crcReminder).ToArray();
        }
        private byte [] CrcReminder(byte[] token, byte[] divisor)
        {
            RightShift(divisor);
            divisor[0] += 128; // first bit must be one

            ChangePosition(token, divisor);
            while (IsNextStep())
            {
                Xor(token, divisor);// xor bit by bit               
                ChangePosition(token, divisor);
            }
            return token.TakeLast(Polynomial.Length).ToArray();
        }

        private void ChangePosition(byte [] token, byte[] divisor)
        {          
            while( BytePosition < Message.Length && (Mask & token[BytePosition]) == 0)
            {
                RightShift(divisor);
                bitPosition++;
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
            return bitPosition < (Message.Length) * 8; 
        }

        private void Xor(byte [] data, byte [] divisor)
        {
            for(int i = data.Length - 1; i >= BytePosition; i--)
            {
                data[i] = (byte)(data[i] ^ divisor[i]);
            }
        }


        public bool Valid(byte[] token, byte [] polynomial) //first bit of token must be one
        {
            bitPosition = 0;
            Message = token.TakeLast(token.Length-polynomial.Length).ToArray();
            Polynomial = polynomial;
            var crcReminder = CrcReminder(token, polynomial.Concat(new byte[Message.Length]).ToArray());
            return crcReminder.All(x => x == 0);
        }
    }
}
