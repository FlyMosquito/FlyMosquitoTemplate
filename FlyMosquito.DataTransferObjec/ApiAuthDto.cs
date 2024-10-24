#region using
#endregion

namespace FlyMosquito.DataTransferObjec
{
    public class ApiAuthDto
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        public int AuthId { get; set; }

        /// <summary>
        /// 权限名
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// API 路由信息
        /// </summary>
        public string RoutePath { get; set; }

        /// <summary>
        /// 动作
        /// </summary>
        public string Action { get; set; }
    }
}
