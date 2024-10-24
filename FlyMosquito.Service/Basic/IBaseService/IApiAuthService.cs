

#region using
using FlyMosquito.Domain;
#endregion

namespace FlyMosquito.Service.Basic.IBaseService
{
    public interface IApiAuthService : IBaseService<ApiAuth>
    {
        Task AddApiAuthAsync(List<ApiAuth> apiAuths);
    }
}
