#region using
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Internal;
using FlyMosquito.Domain;
using System.Reflection;
using System.Xml.Linq;
#endregion

namespace FlyMosquito.Common
{
    public class ApiRouteInfoHelper
    {
        private readonly IActionDescriptorCollectionProvider ApiDescriptionGroupCollectionProvider;

        public ApiRouteInfoHelper(IActionDescriptorCollectionProvider apiDescriptionGroupCollectionProvider)
        {
            ApiDescriptionGroupCollectionProvider = apiDescriptionGroupCollectionProvider;
        }

        /// <summary>
        /// 获取所有的路由信息
        /// </summary>
        /// <returns></returns>
        public List<ApiRouteInfo> GetAllApiRoutes()
        {
            var ApiDescriptionGroups = ApiDescriptionGroupCollectionProvider.ActionDescriptors.Items;

            var ListApiRouteInfo = new List<ApiRouteInfo>();
            var xmlComments = LoadXmlComments();//加载 XML 文档

            foreach (var descriptor in ApiDescriptionGroups)
            {
                var ApiRouteInfo = new ApiRouteInfo
                {
                    GroupName = "", // 根据需要设置组名
                    AreaName = descriptor.RouteValues.TryGetValue("area", out var area) ? area : string.Empty,
                    ControllerFullName = descriptor.RouteValues["controller"],
                    ActionName = descriptor.RouteValues["action"],
                    RealAction = descriptor.AttributeRouteInfo?.Template ?? descriptor.AttributeRouteInfo?.Name,
                    ActionDescription = GetActionDescription(xmlComments, descriptor.RouteValues["action"]),
                    Method = string.Join(", ", descriptor.ActionConstraints?.OfType<HttpMethodActionConstraint>().FirstOrDefault()?.HttpMethods ?? new List<string>()),
                    RoutePath = descriptor.AttributeRouteInfo?.Template,
                    FullName = $"/{descriptor.AttributeRouteInfo?.Template}"
                };

                ListApiRouteInfo.Add(ApiRouteInfo);
            }
            return ListApiRouteInfo;
        }

        /// <summary>
        /// 加载XML文件
        /// </summary>
        /// <returns></returns>
        private XDocument LoadXmlComments()
        {
            var basePath = AppContext.BaseDirectory;//获取当前应用程序的目录
            var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";//获取 XML 注释文件路径
            var xmlPath = Path.Combine(basePath, xmlFile);
            return XDocument.Load(xmlPath);
        }

        /// <summary>
        /// 获取接口描述
        /// </summary>
        /// <param name="xmlComments"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        private string GetActionDescription(XDocument xmlComments, string memberName)
        {
            var StringActionDescription = xmlComments.Descendants("member").FirstOrDefault(m => m.Attribute("name").Value.Contains(memberName))?.Element("summary")?.Value.Trim();//根据 memberName 查找 XML 中的描述
            return StringActionDescription ?? "No description available";
        }
    }
}
