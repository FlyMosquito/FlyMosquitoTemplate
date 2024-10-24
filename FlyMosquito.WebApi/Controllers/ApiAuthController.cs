#region using
using Microsoft.AspNetCore.Mvc;
using FlyMosquito.Common;
using FlyMosquito.Domain;
using FlyMosquito.Service.Basic.IBaseService;
#endregion

namespace FlyMosquito.WebApi.Controllers
{
    [ApiController]
    public class ApiAuthController : ControllerBase
    {
        private readonly ApiRouteInfoHelper ApiRouteInfoService;
        private readonly IApiAuthService ApiAuthService;

        /// <summary>
        /// 用户构造函数
        /// </summary>
        /// <param name="apiRouteInfoService"></param>
        /// <param name="apiAuthService"></param>
        public ApiAuthController(ApiRouteInfoHelper apiRouteInfoService, IApiAuthService apiAuthService)
        {
            this.ApiRouteInfoService = apiRouteInfoService;
            this.ApiAuthService = apiAuthService;
        }

        /// <summary>
        /// 获取所有接口信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ApiAuth/GetApiAuthAsync")]
        public async Task<ApiResult<List<ApiAuth>>> GetApiAuthAsync()
        {
            var ListApiAuth = await ApiAuthService.GetListAsync(); //获取所有的ApiAuth
            var Result = ApiRouteInfoService.GetAllApiRoutes();//获取所有路由信息（从xml里面）
            var ListApiInfo = new List<ApiAuth>();
            foreach (var Item in Result)
            {
                var ApiInfoModel = new ApiAuth()
                {
                    AuthName = Item.ActionName,
                    RoutePath = Item.FullName,
                    Controller = Item.ControllerFullName,
                    Action = Item.Method,
                    ActionDescription = Item.ActionDescription,
                };
                ListApiInfo.Add(ApiInfoModel);
            }
            if (ListApiAuth.Count > 0)
            {
                await ApiAuthService.AddApiAuthAsync(ListApiInfo);//写入数据（刷新ApiAuth权限表） 
            }
            else
            {
                await ApiAuthService.InsertAsync(ListApiInfo);
            }
            ListApiAuth = ApiAuthService.GetList();//再次查询数据
            return ApiResult<List<ApiAuth>>.Success(ListApiAuth);
        }
    }
}
