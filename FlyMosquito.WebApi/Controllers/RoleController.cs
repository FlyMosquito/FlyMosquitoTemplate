#region using
using Microsoft.AspNetCore.Mvc;
using FlyMosquito.DataTransferObjec;
using FlyMosquito.Domain;
using FlyMosquito.Service.Basic.IBaseService;
#endregion

namespace FlyMosquito.WebApi.Controllers
{
    /// <summary>
    /// 角色控制器
    /// </summary>
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService RoleService;
        private readonly IRoleApiAuthMappingService RoleApiAuthMappingService;

        /// <summary>
        /// 角色控制器构造函数
        /// </summary>
        /// <param name="roleService"></param>
        /// <param name="roleApiAuthMappingService"></param>
        public RoleController(IRoleService roleService, IRoleApiAuthMappingService roleApiAuthMappingService)
        {
            this.RoleService = roleService;
            this.RoleApiAuthMappingService = roleApiAuthMappingService;
        }

        /// <summary>
        /// 获取所有角色信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Role/GetRoleAsync")]
        public async Task<ApiResult<List<Role>>> GetRoleAsync()
        {
            var ListRole = await RoleService.GetListAsync();
            return ApiResult<List<Role>>.Success(ListRole);
        }

        /// <summary>
        /// 通过角色ID获取角色信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Role/GetRoleByIdAsync")]
        public async Task<ApiResult<List<Role>>> GetRoleByIdAsync(int Id)
        {
            var ListRole = await RoleService.GetListAsync(x => x.Id == Id);
            return ApiResult<List<Role>>.Success(ListRole);
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="RoleName">角色姓名</param>
        /// <returns>操作结果</returns>
        [HttpPost]
        [Route("Role/AddRoleAsync")]
        public async Task<ApiResult<Role>> AddRoleAsync(string RoleName)
        {
            if (string.IsNullOrWhiteSpace(RoleName))
            {
                return ApiResult<Role>.Fail("角色名称不能为空");
            }
            var ExistingRole = await RoleService.GetAsync(x => x.RoleName == RoleName);
            if (ExistingRole != null)
            {
                return ApiResult<Role>.Fail("角色信息已存在");
            }
            var NewRole = new Role { RoleName = RoleName };
            var InsertResult = await RoleService.InsertAsync(NewRole);
            if (InsertResult == 1)
            {
                return ApiResult<Role>.Success(NewRole);
            }
            return ApiResult<Role>.Fail("新增角色失败，请稍后重试");
        }

        /// <summary> 
        /// 删除角色
        /// </summary>
        /// <param name="Id">角色ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Role/DeleteRoleAsync")]
        public async Task<ApiResult<bool>> DeleteRoleAsync(int Id)
        {
            return await RoleService.DeleteRoleAsync(Id);
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="Role">需要编辑角色信息的实体</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Role/UpdateRoleAsync")]
        public async Task<ApiResult<Role>> UpdateRoleAsync([FromBody] Role Role)
        {
            if (Role == null)
            {
                return ApiResult<Role>.Fail("角色信息不能为空");
            }
            var ExistingRole = await RoleService.GetAsync(x => x.Id != Role.Id && x.RoleName == Role.RoleName);
            if (ExistingRole != null)
            {
                return ApiResult<Role>.Fail("修改后的角色名称已存在，请重新输入");
            }
            var IntUpdateResult = await RoleService.UpdateAsync(Role);
            if (IntUpdateResult >= 1)
            {
                return ApiResult<Role>.Success(Role);
            }
            return ApiResult<Role>.Fail("角色信息修改失败，请重试");
        }

        /// <summary>
        /// 获取角色对应的权限
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>'
        [HttpGet]
        [Route("Role/GetRoleApiAuthAsync")]
        public async Task<ApiResult<List<RoleApiAuthMapping>>> GetRoleApiAuthAsync(int RoleId)
        {
            var ListRoleApiAuthMapping = await RoleApiAuthMappingService.GetListAsync(x => x.RoleId == RoleId);
            return ApiResult<List<RoleApiAuthMapping>>.Success(ListRoleApiAuthMapping);
        }

        /// <summary>
        /// 更新角色对应的权限
        /// </summary>
        /// <param name="RoleApiAuthMappingDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Role/AssignRoleApiAuthAsync")]
        public async Task<ApiResult<bool>> AssignRoleApiAuthAsync([FromBody] RoleApiAuthMappingDto RoleApiAuthMappingDto)
        {
            var Result = await RoleApiAuthMappingService.AssignRoleApiAuthAsync(RoleApiAuthMappingDto.RoleId, RoleApiAuthMappingDto.RoleApiAuthMappingIds);
            return Result;
        }
    }
}
