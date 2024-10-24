#region using
using Microsoft.AspNetCore.Authorization;
#endregion

namespace FlyMosquito.Extension.Authorizations
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string PermissionName { get; }

        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }

        public List<Permission> Permissions { get; set; }
    }
}
