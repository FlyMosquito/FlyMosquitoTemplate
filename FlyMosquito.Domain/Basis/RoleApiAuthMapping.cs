#region using
#endregion

namespace FlyMosquito.Domain
{
    public class RoleApiAuthMapping : BaseModel<int>
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 权限ID
        /// </summary>
        public int AuthId { get; set; }
    }

    public enum RoleApiAuthMappingUpdateState
    {
        None,
        Delete,
        Update,
    }
}
