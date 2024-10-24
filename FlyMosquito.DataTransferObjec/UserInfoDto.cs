#region using
#endregion

namespace FlyMosquito.DataTransferObjec
{
    public class UserInfoDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 登陆账号
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 角色列表，包含每个角色的权限信息
        /// </summary>
        public List<RoleWithApiAuthsDto> Roles { get; set; }
    }
}
