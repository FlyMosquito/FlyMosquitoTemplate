#region using
#endregion

namespace FlyMosquito.Domain
{
    public class LoginLog : BaseModel<int>
    {
        /// <summary>
        /// Desc:用户名
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string? UserName { get; set; }

        /// <summary>
        /// Desc:登录账号
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string? LoginAccount { get; set; }

        /// <summary>
        /// Desc:主键
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Token { get; set; } = null!;

        /// <summary>
        /// Desc:登录IP
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string? LoginIP { get; set; }

        /// <summary>
        /// Desc:登陆地点
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string? LoginLocation { get; set; }

        /// <summary>
        /// Desc:登录时间
        /// Default:
        /// Nullable:False
        /// </summary>           
        public DateTime? LoginTime { get; set; }

        /// <summary>
        /// Desc:退出时间
        /// Default:
        /// Nullable:False
        /// </summary>           
        public DateTime? ExitingTime { get; set; }
    }
}
