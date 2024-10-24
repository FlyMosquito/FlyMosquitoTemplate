#region using
#endregion

namespace FlyMosquito.Domain
{
    public class WebApiLog : BaseModel<int>
    {
        public string Controller { get; set; }

        public string Action { get; set; }

        public string Request { get; set; }

        public string Response { get; set; }

        public string LoginAccount { get; set; }

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

        public DateTime CreateTime { get; set; }
    }
}
