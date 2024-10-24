#region using
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FlyMosquito.Common;
using FlyMosquito.DataTransferObjec;
using FlyMosquito.Domain;
using FlyMosquito.Service.Basic.IBaseService;

#endregion

namespace FlyMosquito.WebApi.Controllers
{
    /// <summary>
    /// 用户控制器
    /// </summary>
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService UserService;
        private readonly IUserRoleMappingService UserRoleMappingService;

        /// <summary>
        /// 用户控制器构造函数
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="userRoleMappingService"></param>
        public UserController(IUserService userService, IUserRoleMappingService userRoleMappingService)
        {
            this.UserService = userService;
            this.UserRoleMappingService = userRoleMappingService;
        }

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("User/GetUserAsync")]
        public async Task<ApiResult<List<User>>> GetUserAsync()
        {
            var ListUser = await UserService.GetUserAsync();
            return ApiResult<List<User>>.Success(ListUser);
        }

        /// <summary>
        /// 获取用户对应的角色信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("User/GetUserRolesAsync")]
        public async Task<ApiResult<List<UserRoleMapping>>> GetUserRolesAsync(int UserId)
        {
            var ListUserRoleMapping = await UserRoleMappingService.GetListAsync(x => x.UserId == UserId);
            return ApiResult<List<UserRoleMapping>>.Success(ListUserRoleMapping);
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns> 
        [HttpPost]
        [Route("User/AddUserAsync")]
        [AllowAnonymous]
        public async Task<ApiResult<UserDto>> AddUserAsync(string UserName, string PassWord)
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(PassWord))
            {
                return ApiResult<UserDto>.Fail("数据异常，请检查!");
            }
            var Result = await UserService.AddUserAsync(UserName, PassWord);
            if (Result != null)
            {
                return ApiResult<UserDto>.Success(Result);
            }

            return ApiResult<UserDto>.Fail("新增用户失败!");
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("User/DeleteUserAsync")]
        public async Task<ApiResult<string>> DeleteUserAsync(int id)
        {
            var User = await UserService.GetAsync(x => x.Id == id);
            User.IsEnable = 0;
            if (User == null)
            {
                return ApiResult<string>.Fail("用户不存在");
            }
            var IntResult = await UserService.UpdateAsync(User);
            if (IntResult != 1)
            {
                return ApiResult<string>.Fail("删除用户信息失败");
            }
            return ApiResult<string>.Success("删除用户信息成功");
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="UserDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("User/UpdateUserAsync")]
        public async Task<ApiResult<string>> UpdateUserAsync([FromBody] UserDto UserDto)
        {
            var User = await UserService.GetAsync(x => x.Id == UserDto.Id);
            if (User == null)
            {
                return ApiResult<string>.Fail("用户数据异常，请检查");
            }
            User.UserName = UserDto.UserName;
            User.PassWord = Md5Helper.MD5Encrypt64(UserDto.PassWord);
            User.IsEnable = UserDto.IsEnable;
            var IntResult = await UserService.UpdateAsync(User);
            if (IntResult != 1)
            {
                return ApiResult<string>.Fail("更新用户信息失败");
            }
            return ApiResult<string>.Success("更新用户信息成功");
        }

        /// <summary>
        /// 为用户分配权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("User/AssignUserRolesAsync")]
        public async Task<ApiResult<bool>> AssignUserRolesAsync([FromBody] UserRoleMappingDto request)
        {
            var Result = await UserRoleMappingService.AssignUserRolesAsync(request.UserId, request.RoleIds);
            return Result;
        }
    }
}
