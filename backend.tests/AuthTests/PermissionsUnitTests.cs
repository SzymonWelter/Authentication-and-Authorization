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
            bool arg = false;
            Permission permission = new Permission(1,arg,!arg,arg,!arg);
            Assert.False(permission.PermissionRequest(new Permission(1,arg,arg,!arg,!arg)));
            Assert.False(permission.PermissionRequest(new Permission(2,arg,!arg,arg,!arg)));
            Assert.False(permission.PermissionRequest(new Permission(1,!arg, arg, arg, arg)));
            Assert.True(permission.PermissionRequest(new Permission(1,arg,!arg,arg,!arg)));
            Assert.True(permission.PermissionRequest(new Permission(1,arg,!arg,arg,arg)));
            Assert.True(permission.PermissionRequest(new Permission(1,arg,arg,arg,!arg)));
        }

        [Fact]
        public void TestPermissions(){
            bool arg = false;
            Permissions permissions = new Permissions();
            var permList = (List<IPermission>)permissions.PermissionsCollection;
            permList.Add(new Permission(1,!arg,!arg,!arg,arg));
            permList.Add(new Permission(2,!arg,!arg,arg,arg));
            permList.Add(new Permission(3,arg,!arg,arg,arg));
            permList.Add(new Permission(4,arg,arg,!arg,arg));
            permList.Add(new Permission(5,arg,!arg,arg,!arg));

            Assert.True(permissions.PermissionRequest(new Permission(5,arg,!arg,arg,!arg)));
            Assert.True(permissions.PermissionRequest(new Permission(4,arg,!arg,arg,arg)));
            Assert.False(permissions.PermissionRequest(new Permission(3,arg,arg,!arg,arg)));
            Assert.False(permissions.PermissionRequest(new Permission(2,!arg,arg,!arg,!arg)));
            
        }
    }
}
