#region using
using FlyMosquito.Core;
using FlyMosquito.Domain;
using FlyMosquito.Service.Basic.IBaseService;
#endregion

namespace FlyMosquito.Service.Basic.BaseService
{
    public class ApiAuthService : BaseService<ApiAuth>, IApiAuthService
    {
        private readonly IRepository<ApiAuth> ApiAuthRepo;
        private readonly IRepository<RoleApiAuthMapping> RoleApiAuthMappingRepo;
        public ApiAuthService(IRepository<ApiAuth> apiAuthRepo, IRepository<RoleApiAuthMapping> roleApiAuthMappingRepo) : base(apiAuthRepo)
        {
            ApiAuthRepo = apiAuthRepo;
            RoleApiAuthMappingRepo = roleApiAuthMappingRepo;
        }

        /// <summary>
        /// 刷新ApiAuth权限表
        /// </summary>
        /// <param name="apiAuths"></param>
        /// <returns></returns>
        public async Task AddApiAuthAsync(List<ApiAuth> apiAuths)
        {
            var ListWillDeleteData = new List<ApiAuth>();
            var ListWillUpdateData = new List<ApiAuth>();
            var ListApiAuth = await ApiAuthRepo.GetListAsync(); //获取所有的ApiAuth

            //检测已有的数据
            foreach (var ApiAuth in ListApiAuth)
            {
                var IsUpdate = ApiAuth.CheckAndUpdateData(apiAuths);
                if (IsUpdate == ApiAuthUpdateState.Delete)
                {
                    ListWillDeleteData.Add(ApiAuth);//删除数据库原本存在的
                }
                else if (IsUpdate == ApiAuthUpdateState.Update)
                {
                    ListWillUpdateData.Add(ApiAuth);
                }
            }

            //筛选要添加的数据
            var NewApiAuth = ListApiAuth.Select(x => x.RoutePath + "_" + x.Action);
            var ListWillAddData = apiAuths.Where(x => !NewApiAuth.Contains(x.RoutePath + "_" + x.Action)).ToList();

            await ApiAuthRepo.UpdateAsync(ListWillUpdateData);//修改
            await ApiAuthRepo.DeleteAsync(x => ListWillDeleteData.Select(y => y.Id).ToList().Contains(x.Id));//删除不存在的
            await ApiAuthRepo.InsertAsync(ListWillAddData);//编辑

            //还要去删除 RoleApiAuthMapping 没有的AuthId
            var ListRoleApiAuthMapping = await RoleApiAuthMappingRepo.GetListAsync();
            ListApiAuth = await ApiAuthRepo.GetListAsync();//
            var ListApiAuthId = ListApiAuth.Select(x => x.Id).ToList();

            //找到不存在的数据然后删除
            var ListDelete = ListRoleApiAuthMapping.Where(x => !ListApiAuthId.Contains(x.AuthId)).Select(x => x.AuthId).ToList();//需要删除的
            await RoleApiAuthMappingRepo.DeleteAsync(x => ListDelete.Contains(x.AuthId));//删除
        }
    }
}
