#region using
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using FlyMosquito.Common;
using FlyMosquito.Domain;
using FlyMosquito.Service.AuditLog.IAuditLogService;
using System.Net;
#endregion

namespace FlyMosquito.Extension.Filter
{
    public class GlobalActionFilter : IActionFilter
    {
        private readonly IWebApiLogService WebApiLogService;
        private string Request;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webApiLogService"></param>
        public GlobalActionFilter(IWebApiLogService webApiLogService)
        {
            WebApiLogService = webApiLogService;
        }

        /// <summary>
        /// 在结果执行后调用，记录请求和响应数据
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            LogRequestAndResponse(context);
        }

        /// <summary>
        /// 在结果执行前调用，保存请求数据
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Request = JsonConvert.SerializeObject(context.ActionArguments);
        }

        private void LogRequestAndResponse(ActionExecutedContext context)
        {
            try
            {
                var StringControllerName = context.RouteData.Values["controller"]?.ToString() ?? "UnknownController";
                var StringActionName = context.RouteData.Values["action"]?.ToString() ?? "UnknownAction";
                var StringResponse = context.Result != null ? JsonConvert.SerializeObject(context.Result) : "NoResponse";
                var StringLoginAccount = context.HttpContext.User.Identity.IsAuthenticated ? context.HttpContext.User.Identity.Name ?? "Anonymous" : "Anonymous";
                var clientIp = GetClientIp(context.HttpContext);

                // 创建 WebApiLog 对象并填充属性
                var webApiLog = new WebApiLog
                {
                    Controller = StringControllerName,
                    Action = StringActionName,
                    Request = Request,
                    Response = StringResponse,
                    LoginAccount = "测试",
                    CreateTime = DateTime.Now,
                    LoginIP = clientIp,
                };

                WebApiLogService.CreateWebApiLog(webApiLog);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"错误API请求和响应{ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 获取客户端 IP 地址
        /// </summary>
        /// <param name="context">HTTP 上下文</param>
        /// <returns>客户端 IP 地址</returns>
        private string GetClientIp(HttpContext context)
        {
            try
            {
                var ip = context.Connection.RemoteIpAddress;

                // 如果是 IPv4 映射到 IPv6，需要转换回来
                if (ip.IsIPv4MappedToIPv6)
                {
                    ip = ip.MapToIPv4();
                }

                // 检查是否有 X-Forwarded-For 头（适用于反向代理）
                if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
                {
                    var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(forwardedFor))
                    {
                        ip = IPAddress.Parse(forwardedFor.Split(',').First().Trim());
                    }
                }

                return ip?.ToString() ?? "UnknownIP";
            }
            catch
            {
                return "UnknownIP";
            }
        }
    }
}
