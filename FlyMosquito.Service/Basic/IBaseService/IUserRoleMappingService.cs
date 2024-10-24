#region using
using FlyMosquito.Domain;
#endregion

namespace FlyMosquito.Service.Basic.IBaseService
{
    public interface IUserRoleMappingService : IBaseService<UserRoleMapping>
    {
        Task<ApiResult<bool>> AssignUserRolesAsync(int UserId, List<int> RoleIds);
    }
}
