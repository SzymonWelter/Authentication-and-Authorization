using System;
using Xunit;
using backend.Auth;
using System.Linq;
using System.Collections.Generic;

namespace backend.tests.AuthTests
{
    public class PermissionsUnitTests
    {

        [Fact]
        public void TestPermission()
        {    
            Permission permission = new Permission(1,false, false, false, false);
            permission.Edit = true;
            Assert.True(permission.PermissionRequest(new Permission(1,false, true, false, false)));
            Assert.True(permission.PermissionRequest(new Permission(1,false, true, true, false)));
            Assert.True(permission.PermissionRequest(new Permission(1,false, false, true, false)));
            Assert.False(permission.PermissionRequest(new Permission(1,false, true, false, true)));
            Assert.False(permission.PermissionRequest(new Permission(2,false,false,true,false)));
            permission.Edit = false;
            permission.Display = true;
            Assert.False(permission.PermissionRequest(new Permission(1,false,false,true,false)));
        }
    }
}
