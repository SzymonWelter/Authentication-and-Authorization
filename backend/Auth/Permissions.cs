using System.Collections.Generic;

namespace backend.Auth
{
    public class Permissions : IPermissions
    {
        public ICollection<IPermission> PermissionsCollection { get; set; }

        public bool PermissionRequest(IPermission permission)
        {
            var en = PermissionsCollection.GetEnumerator();
            while(en.MoveNext())
                if(en.Current.Id == permission.Id)
                    return en.Current.PermissionRequest(permission); 
            return false;
        }
    }

}