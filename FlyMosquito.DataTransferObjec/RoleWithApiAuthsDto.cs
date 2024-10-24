#region using
#endregion

namespace FlyMosquito.DataTransferObjec
{
    public class RoleWithApiAuthsDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 角色姓名
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色对应的API权限映射
        /// </summary>
        public List<ApiAuthDto> ApiAuths { get; set; }
    }
}
