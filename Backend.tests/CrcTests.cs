using backend.TokenServices;
using System;
using Xunit;

namespace Backend.tests
{
    public class CrcTests
    {
        private readonly Crc crc;
        public CrcTests()
        {
            crc = new Crc();
        }

        [Fact]
        public void IsValid()
        {
            var divisor = new byte[] { 192, 168, 8, 101 }; ;
            var message = new byte[] { 101, 111, 51, 14, 81, 12, 128 };
            crc.Message = message;
            crc.Divisor = divisor;
            var token = crc.Encode();
            Assert.True(crc.Valid(token, divisor));
        }
    }
}
