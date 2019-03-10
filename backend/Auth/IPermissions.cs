
using System.Collections.Generic;

namespace backend.Auth
{
    public interface IPermissions   
    {
        ICollection<IPermission> PermissionsCollection { get; set; }
        bool PermissionRequest(IPermission permission) ;
    }    
}
