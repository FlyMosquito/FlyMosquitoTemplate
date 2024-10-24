namespace FlyMosquito.Domain
{
    /// <summary>
    /// webapi路由信息
    /// </summary>
    public class ApiRouteInfo
    {
        /// <summary>
        /// 代表 API 的分组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 代表 API 所属的区域名
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 表示控制器的名称
        /// </summary>
        public string ControllerFullName { get; set; }

        /// <summary>
        /// 表示控制器中的具体操作方法名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 实际操作名称
        /// </summary>
        public string RealAction { get; set; }

        /// <summary>
        /// 描述 API 操作的字符串
        /// </summary>
        public string ActionDescription { get; set; }

        /// <summary>
        /// 表示 HTTP 方法（如 GET, POST, PUT, DELETE 等）
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 表示 API 路由的相对路径
        /// </summary>
        public string RoutePath { get; set; }

        /// <summary>
        /// 一个组合字段，用于显示 HTTP 方法和相对路径的完整信息
        /// </summary>
        public string FullName { get; set; }
    }
}
