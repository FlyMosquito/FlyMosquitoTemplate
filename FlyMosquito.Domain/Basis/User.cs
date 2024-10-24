#region using
#endregion

namespace FlyMosquito.Domain
{
    public class User : BaseModel<int>
    {
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登陆账号
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 是否启用 0可用（默认） 1禁用
        /// </summary>
        public int? IsEnable { get; set; }
    }
}
