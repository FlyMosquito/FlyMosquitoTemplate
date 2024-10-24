#region using
#endregion

namespace FlyMosquito.DataTransferObjec
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 登陆账号
        /// </summary>
        public string? Uid { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int IsEnable { get; set; }
    }
}
