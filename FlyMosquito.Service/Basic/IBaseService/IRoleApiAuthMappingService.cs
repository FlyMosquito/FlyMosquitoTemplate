#region using
using FlyMosquito.DataTransferObjec;
using FlyMosquito.Domain;
#endregion

namespace FlyMosquito.Service.Basic.IBaseService
{
    public interface IRoleApiAuthMappingService : IBaseService<RoleApiAuthMapping>
    {
        Task<ApiResult<bool>> AssignRoleApiAuthAsync(int RoleId, List<int> ApiAuthMappingIds);
        Task<List<RoleWithApiAuthsDto>> GetRoleApiAuthMappingAsync();
    }
}
