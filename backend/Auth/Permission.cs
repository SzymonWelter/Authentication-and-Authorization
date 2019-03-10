namespace backend.Auth
{
    public class Permission : IPermission
    {
        public int Id { get ; set ; }
        public bool Create { get; set; }
        public bool Display { get; set; }   
        public bool Edit { get; set; }
        public bool Remove { get; set; }     

        public bool PermissionRequest(IPermission permission)
        {
            if (typeof(Permission) != permission.GetType())
                return false;

            var perm = (Permission) permission;
            return (!perm.Create || Create) &&
                (!perm.Display || Display) &&
                (!perm.Edit || Edit) &&
                (!perm.Remove || Remove);
        }
    }
}
