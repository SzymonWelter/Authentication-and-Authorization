using System;
using Xunit;
using backend.Auth;

namespace backend.tests.AuthTests
{
    public class PermissionsUnitTests
    {

        [Fact]
        public void TestPermission()
        {    
            bool arg = false;
            Permission permission = new Permission(1,arg,!arg,!arg,arg);
            Assert.False(permission.PermissionRequest(new Permission(1,!arg,arg,arg,arg)));
            Assert.True(permission.PermissionRequest(new Permission(1,arg,!arg,arg,arg)));
            Assert.False(permission.PermissionRequest(new Permission(2,arg,!arg,arg,arg)));
        }
    }
}
