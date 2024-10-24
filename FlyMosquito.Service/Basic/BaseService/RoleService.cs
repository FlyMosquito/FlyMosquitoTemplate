#region using
using FlyMosquito.Common;
using FlyMosquito.Core;
using FlyMosquito.Domain;
using FlyMosquito.Service.Basic.IBaseService;
#endregion

namespace FlyMosquito.Service.Basic.BaseService
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        private readonly IRepository<UserRoleMapping> UserRoleMappingRepo;
        private readonly IRepository<RoleApiAuthMapping> RoleApiAuthMappingRepo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleRepo"></param>
        /// <param name="userRoleMappingRepo"></param>
        /// <param name="roleApiAuthMappingRepo"></param>
        public RoleService(IRepository<Role> roleRepo, IRepository<UserRoleMapping> userRoleMappingRepo, IRepository<RoleApiAuthMapping> roleApiAuthMappingRepo) : base(roleRepo)
        {
            UserRoleMappingRepo = userRoleMappingRepo;
            RoleApiAuthMappingRepo = roleApiAuthMappingRepo;
        }

        /// <summary>
        /// 检查是否可以删除角色
        /// </summary>
        /// <param name="IntRoleId">需要检查的角色</param>
        /// <returns>是否可以删除角色，若角色有用户关联则返回 false，否则返回 true</returns>
        public async Task<ApiResult<bool>> DeleteRoleAsync(int IntRoleId)
        {
            try
            {
                await ExecuteInTransactionAsync(async () =>
                {
                    var Role = await GetAsync(x => x.Id == IntRoleId);
                    if (Role == null)
                    {
                        throw new Exception("角色信息不存在");
                    }

                    var ListUserRoleMapping = await UserRoleMappingRepo.GetListAsync(x => x.RoleId == IntRoleId);
                    if (ListUserRoleMapping.Any())
                    {
                        throw new Exception("选择删除的角色已经分配人员信息，不允许删除！");
                    }

                    var BoolDeleteRoleResult = await DeleteAsync(IntRoleId);
                    if (!BoolDeleteRoleResult)
                    {
                        throw new Exception("删除角色失败");
                    }

                    var DeleteMappingsResult = await RoleApiAuthMappingRepo.DeleteAsync(x => x.RoleId == IntRoleId);
                    if (DeleteMappingsResult <= 0)
                    {
                        throw new Exception("删除角色权限映射失败");
                    }
                });

                return ApiResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                LoggerHelper.Error($"删除角色失败{ex.Message}");
                return ApiResult<bool>.Fail(ex.Message);
            }
        }
    }
}
