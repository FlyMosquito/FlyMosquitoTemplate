#region using
using AutoMapper;
using FlyMosquito.Common;
using FlyMosquito.Core;
using FlyMosquito.DataTransferObjec;
using FlyMosquito.Domain;
using FlyMosquito.Service.Basic.IBaseService;

#endregion

namespace FlyMosquito.Service.Basic.BaseService
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IRepository<User> UserRepo;
        private readonly IRepository<UserRoleMapping> UserRoleMappingRepo;
        private readonly IRepository<Role> RoleRepo;
        private readonly IRepository<RoleApiAuthMapping> RoleApiAuthMappingRepo;
        private readonly IRepository<ApiAuth> ApiAuthRepo;
        public IMapper Mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRepo"></param>
        /// <param name="userRoleMappingRepo"></param>
        /// <param name="roleRepo"></param>
        /// <param name="roleApiAuthMappingRepo"></param>
        /// <param name="apiAuthRepo"></param>
        public UserService(IRepository<User> userRepo,
            IRepository<UserRoleMapping> userRoleMappingRepo,
            IRepository<Role> roleRepo,
            IRepository<RoleApiAuthMapping> roleApiAuthMappingRepo,
            IRepository<ApiAuth> apiAuthRepo,
            IMapper mapper) : base(userRepo)
        {
            UserRepo = userRepo;
            UserRoleMappingRepo = userRoleMappingRepo;
            RoleRepo = roleRepo;
            RoleApiAuthMappingRepo = roleApiAuthMappingRepo;
            ApiAuthRepo = apiAuthRepo;
            Mapper = mapper;
        }

        #region 用户登陆
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<ApiResult<UserInfoDto>> LoginAsync(string uid, string password)
        {
            try
            {
                var User = await UserRepo.GetAsync(x => x.Uid == uid && x.PassWord == Md5Helper.MD5Encrypt64(password) && x.IsEnable == 1);//查询用户信息
                if (User == null)
                {
                    return ApiResult<UserInfoDto>.Fail("账号异常");
                }
                var ListUserRoleMapping = await UserRoleMappingRepo.GetListAsync(x => x.UserId == User.Id);//获取用户的角色映射
                var ListRoleId = ListUserRoleMapping.Select(x => x.RoleId).Distinct().ToList();//查询用户拥有的角色信息
                var ListRole = await RoleRepo.GetListAsync(x => ListRoleId.Contains(x.Id));

                if (!ListRoleId.Any())
                {
                    var UserInfoDto = new UserInfoDto
                    {
                        UserId = User.Id,
                        Uid = User.Uid,
                        UserName = User.UserName,
                        Roles = new List<RoleWithApiAuthsDto>()//用户没有分配任何角色
                    };
                    return ApiResult<UserInfoDto>.Success(UserInfoDto);
                }
                else
                {
                    var ListRoleApiAuthMapping = await RoleApiAuthMappingRepo.GetListAsync(x => ListRoleId.Contains(x.RoleId));//查询角色对应的API权限信息
                    var ListAuthIds = ListRoleApiAuthMapping.Select(x => x.AuthId).Distinct().ToList();//根据API ID找出具体权限信息
                    var ListApiAuths = await ApiAuthRepo.GetListAsync(x => ListAuthIds.Contains(x.Id));

                    // 构建用户角色和API权限信息
                    var ListUserRolesWithApiAuths = ListRole.Select(role => new RoleWithApiAuthsDto
                    {
                        RoleId = role.Id,
                        RoleName = role.RoleName,
                        ApiAuths = ListRoleApiAuthMapping
                                    .Where(mapping => mapping.RoleId == role.Id)
                                    .Select(mapping =>
                                    {
                                        var apiAuth = ListApiAuths.FirstOrDefault(auth => auth.Id == mapping.AuthId);
                                        return new ApiAuthDto
                                        {
                                            AuthId = apiAuth?.Id ?? 0,
                                            Controller = apiAuth?.Controller,
                                            RoutePath = apiAuth?.RoutePath,
                                            Action = apiAuth?.Action,
                                        };
                                    }).ToList()
                    }).ToList();

                    var UserInfoDto = new UserInfoDto
                    {
                        UserId = User.Id,
                        Uid = User.Uid,
                        UserName = User.UserName,
                        Token = GenerateToken(User, ListRole),//生成Token的逻辑
                        Roles = ListUserRolesWithApiAuths
                    };

                    return ApiResult<UserInfoDto>.Success(UserInfoDto);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.Message);
                return ApiResult<UserInfoDto>.Fail("登陆失败，系统异常");
            }
        }

        /// <summary>
        /// 获取用户token
        /// </summary>
        /// <param name="User"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        private string GenerateToken(User User, List<Role> roles)
        {
            var Token = JwtTokenHelper.GetToken(User.Uid, User.UserName, roles.Select(x => x.RoleName).ToList());
            return Token;
        }
        #endregion

        #region 新增用户
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="userDto">用户信息</param>
        /// <returns>新增后的用户信息</returns>
        public async Task<UserDto> AddUserAsync(string UserName, string PassWord)
        {
            var user = new User()
            {
                UserName = UserName,
                Uid = CreateUid(),
                PassWord = Md5Helper.MD5Encrypt64(PassWord),
                IsEnable = 1
            };

            var InsertResult = await UserRepo.InsertAsync(user);
            if (InsertResult == 1)
            {
                var Result = Mapper.Map<UserDto>(user);//数据映射
                return Result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 生成会员ID随机数300000~9999999
        /// </summary>
        /// <returns></returns>
        public string CreateUid()
        {
            Random rand = new Random((int)(DateTime.Now.Ticks % 1000000));//随机函数
            var StringUid = $"UID{rand.Next(300000, 999999)}";
            //判断数据是否存在
            var User = UserRepo.Get(x => x.Uid == StringUid);
            if (User != null)
            {
                StringUid = User.Uid;
            }
            return StringUid;
        }
        #endregion

        #region 获取用户信息
        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<User>> GetUserAsync()
        {
            var ListUser = await UserRepo.GetListAsync(x => x.IsEnable == 1);
            return ListUser;
        }
        #endregion
    }
}
