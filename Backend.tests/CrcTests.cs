using backend.TokenServices;
using System;
using System.Collections.Generic;
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
            var polynomial = new byte[] { 192, 168, 8, 101 };
            var message = new byte[] { 101, 111, 51, 14, 81, 12, 128 };
            var token = crc.Encode(message, polynomial);
            Assert.True(crc.Valid(new List<byte>(token).ToArray() , new byte[] { 192, 168, 8, 101 }));
            Assert.False(crc.Valid(new List<byte>(token).ToArray(), new byte[] { 111, 111, 111, 111}));
            Assert.False(crc.Valid(new List<byte>(token).ToArray(), new byte[] { 192, 168, 8, 102 }));        
        }
    }
}
