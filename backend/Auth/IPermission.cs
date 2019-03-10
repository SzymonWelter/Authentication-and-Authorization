namespace backend.Auth
{
    public interface IPermission
    {
        int Id { get; set; }
        bool PermissionRequest(IPermission permission);    
    }    
}
