#region using
using FlyMosquito.Domain;
#endregion

namespace FlyMosquito.Service.Basic.IBaseService
{
    public interface IRoleService : IBaseService<Role>
    {
        Task<ApiResult<bool>> DeleteRoleAsync(int roleId);
    }
}
