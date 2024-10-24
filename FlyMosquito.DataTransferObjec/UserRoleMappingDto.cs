#region using
#endregion

namespace FlyMosquito.DataTransferObjec
{
    public class UserRoleMappingDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 待分配的角色
        /// </summary>
        public List<int> RoleIds { get; set; }
    }
}
