#region using 
#endregion

namespace FlyMosquito.Domain
{
    public class ApiAuth : BaseModel<int>
    {
        public string Controller { get; set; }

        public string AuthName { get; set; }

        public string RoutePath { get; set; }

        public string? Action { get; set; }

        public string? ActionDescription { get; set; }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <param name="ApiAuths"></param>
        /// <returns></returns>
        public ApiAuthUpdateState CheckAndUpdateData(List<ApiAuth> ApiAuths)
        {
            //看数据是否存在
            var ApiAuth = ApiAuths.FirstOrDefault(x => x.Controller == Controller && x.AuthName == AuthName && x.Action == Action && x.RoutePath == RoutePath);

            //新增加的数据已经存在了，那么就更新数据
            if (ApiAuth != null)
            {
                return UpdateData(ApiAuth);
            }
            else
            {
                return ApiAuthUpdateState.Delete;//代表最新的控制器  数据库没有这条数据，就要删除了
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="ApiAuth"></param>
        /// <returns></returns>
        public ApiAuthUpdateState UpdateData(ApiAuth ApiAuth)
        {
            if (ActionDescription == ApiAuth.ActionDescription)
            {
                return ApiAuthUpdateState.None;
            }
            else
            {
                ActionDescription = ApiAuth.ActionDescription;
                return ApiAuthUpdateState.Update;
            }
        }
    }

    public enum ApiAuthUpdateState
    {
        None,
        Delete,
        Update,
    }
}
