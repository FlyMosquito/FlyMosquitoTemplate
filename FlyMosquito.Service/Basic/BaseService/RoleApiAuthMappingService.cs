#region using
using FlyMosquito.Common;
using FlyMosquito.Core;
using FlyMosquito.DataTransferObjec;
using FlyMosquito.Domain;
using FlyMosquito.Service.Basic.IBaseService;
#endregion

namespace FlyMosquito.Service.Basic.BaseService
{
    public class RoleApiAuthMappingService : BaseService<RoleApiAuthMapping>, IRoleApiAuthMappingService
    {
        private readonly IRepository<RoleApiAuthMapping> RoleApiAuthMappingRepo;
        private readonly IRepository<Role> RoleRepo;
        private readonly IRepository<ApiAuth> ApiAuthRepo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleApiAuthMappingRepo"></param>
        /// <param name="roleRepo"></param>
        /// <param name="apiAuthRepo"></param>
        public RoleApiAuthMappingService(IRepository<RoleApiAuthMapping> roleApiAuthMappingRepo, IRepository<Role> roleRepo, IRepository<ApiAuth> apiAuthRepo) : base(roleApiAuthMappingRepo)
        {
            RoleApiAuthMappingRepo = roleApiAuthMappingRepo;
            RoleRepo = roleRepo;
            ApiAuthRepo = apiAuthRepo;
        }

        /// <summary>
        /// 获取角色对应的信息API
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleWithApiAuthsDto>> GetRoleApiAuthMappingAsync()
        {
            //获取所有角色分配的API信息
            var ListRoleApiAuthMapping = await RoleApiAuthMappingRepo.GetListAsync();
            var ListAuthIds = ListRoleApiAuthMapping.Select(x => x.AuthId).Distinct().ToList();

            //获取详情信息
            var ListApiAuths = await ApiAuthRepo.GetListAsync(x => ListAuthIds.Contains(x.Id));

            //获取角色信息
            var ListRoleId = ListRoleApiAuthMapping.Select(x => x.RoleId).ToList();
            var ListRoles = await RoleRepo.GetListAsync(x => ListRoleId.Contains(x.Id));

            // 构建用户角色和API权限信息
            var ListUserRolesWithApiAuths = ListRoles.Select(role => new RoleWithApiAuthsDto
            {
                RoleId = role.Id,
                RoleName = role.RoleName,
                ApiAuths = ListRoleApiAuthMapping.Where(mapping => mapping.RoleId == role.Id).Select(mapping =>
                            {
                                var apiAuth = ListApiAuths.FirstOrDefault(auth => auth.Id == mapping.AuthId);
                                return new ApiAuthDto
                                {
                                    AuthId = apiAuth?.Id ?? 0,
                                    Controller = apiAuth?.Controller,
                                    RoutePath = apiAuth?.RoutePath,
                                    Action = apiAuth?.Action
                                };
                            }).ToList()
            }).ToList();

            return ListUserRolesWithApiAuths;
        }

        /// <summary>
        /// 更新角色对应的权限
        /// </summary>
        /// <param name="RoleId">角色ID</param>
        /// <param name="ApiAuthMappingIds"></param>
        /// <returns></returns>
        public async Task<ApiResult<bool>> AssignRoleApiAuthAsync(int RoleId, List<int> ApiAuthMappingIds)
        {
            if (ApiAuthMappingIds == null)
            {
                return ApiResult<bool>.Fail("权限ID列表不能为空");
            }
            try
            {
                var Role = await RoleRepo.GetAsync(x => x.Id == RoleId);
                if (Role == null)
                {
                    throw new Exception("角色不存在");
                }

                //获取这个角色已经分配的权限数据
                var ListRoleApiAuthMapping = await RoleApiAuthMappingRepo.GetListAsync(x => x.RoleId == RoleId);
                var ListAuthId = ListRoleApiAuthMapping.Select(x => x.AuthId).ToList();

                //计算需要删除的权限映射（存在于数据库中，但不在新的权限列表中）
                var ListDeleteAuthId = ListAuthId.Except(ApiAuthMappingIds).ToList();
                var ListMappingsToDelete = ListRoleApiAuthMapping.Where(x => ListDeleteAuthId.Contains(x.AuthId)).ToList();

                //计算需要新增的权限映射（存在于新的权限列表中，但不在数据库中）
                var ListAddAuthId = ApiAuthMappingIds.Except(ListAuthId).ToList();
                var ListMappingsToAdd = ListAddAuthId.Select(x => new RoleApiAuthMapping
                {
                    RoleId = RoleId,
                    AuthId = x
                }).ToList();

                //开启事务
                await ExecuteInTransactionAsync(async () =>
                {
                    if (ListMappingsToDelete.Any())
                    {
                        await RoleApiAuthMappingRepo.DeleteRangeAsync(ListMappingsToDelete);
                    }

                    if (ListMappingsToAdd.Any())
                    {
                        await RoleApiAuthMappingRepo.InsertAsync(ListMappingsToAdd);
                    }
                });

                return ApiResult<bool>.Success();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"更新角色权限失败:{ex.Message}");
                return ApiResult<bool>.Fail(ex.Message);
            }
        }
    }
}
