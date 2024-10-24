#region using
using FlyMosquito.DataTransferObjec;
using FlyMosquito.Domain;
#endregion

namespace FlyMosquito.Service.Basic.IBaseService
{
    public interface IUserService : IBaseService<User>
    {
        Task<UserDto> AddUserAsync(string UserName, string PassWord);
        Task<List<User>> GetUserAsync();
        Task<ApiResult<UserInfoDto>> LoginAsync(string uid, string password);
    }
}
