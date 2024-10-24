#region using
#endregion

namespace FlyMosquito.Extension.Authorizations
{
    public class Permission
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Controller { get; set; }
        public string RoutePath { get; set; }
        public string Action { get; set; }
    }
}
