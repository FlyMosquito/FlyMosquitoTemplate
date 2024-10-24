#region using
using FlyMosquito.Common;
using FlyMosquito.Core;
using FlyMosquito.Domain;
using FlyMosquito.Service.Basic.IBaseService;
#endregion

namespace FlyMosquito.Service.Basic.BaseService
{
    public class UserRoleMappingService : BaseService<UserRoleMapping>, IUserRoleMappingService
    {
        private readonly IRepository<UserRoleMapping> UserRoleMappingRepo;
        private readonly IRepository<User> UserRepo;

        public UserRoleMappingService(IRepository<UserRoleMapping> userRoleMappingRepo, IRepository<User> userRepo) : base(userRoleMappingRepo)
        {
            UserRoleMappingRepo = userRoleMappingRepo;
            UserRepo = userRepo;
        }

        /// <summary>
        /// 给用户分配角色
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="RoleIds"></param>
        /// <returns></returns>
        public async Task<ApiResult<bool>> AssignUserRolesAsync(int UserId, List<int> RoleIds)
        {
            if (RoleIds == null)
            {
                return ApiResult<bool>.Fail("角色ID列表不能为空");
            }
            try
            {
                var User = await UserRepo.GetAsync(x => x.Id == UserId);
                if (User == null)
                {
                    throw new Exception("用户不存在");
                }

                //获取这个用户已经分配的角色数据
                var ListUserRoleMapping = await UserRoleMappingRepo.GetListAsync(x => x.UserId == UserId);
                var ListRoleId = ListUserRoleMapping.Select(x => x.RoleId).ToList();

                //计算需要删除的权限映射（存在于数据库中，但不在新的角色列表中）
                var ListDeleteRoleId = ListRoleId.Except(RoleIds).ToList();
                var ListMappingsToDelete = ListUserRoleMapping.Where(x => ListDeleteRoleId.Contains(x.RoleId)).ToList();

                //计算需要新增的角色映射（存在于新的角色列表中，但不在数据库中）
                var ListAddRoleId = RoleIds.Except(ListRoleId).ToList();
                var ListMappingsToAdd = ListAddRoleId.Select(x => new UserRoleMapping
                {
                    RoleId = x,
                    UserId = UserId,
                }).ToList();

                //开启事务
                await ExecuteInTransactionAsync(async () =>
                {
                    if (ListMappingsToDelete.Any())
                    {
                        await UserRoleMappingRepo.DeleteRangeAsync(ListMappingsToDelete);
                    }

                    if (ListMappingsToAdd.Any())
                    {
                        await UserRoleMappingRepo.InsertAsync(ListMappingsToAdd);
                    }
                });

                return ApiResult<bool>.Success();
            }
            catch (Exception ex)
            {
                LoggerHelper.Error("更新用户角色权限失败");
                return ApiResult<bool>.Fail(ex.Message);
            }
        }
    }
}
