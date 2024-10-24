#region using
using Microsoft.IdentityModel.Tokens;
using FlyMosquito.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
#endregion

namespace FlyMosquito.Common
{
    public class JwtTokenHelper
    {
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static string GetToken(string userId, string userName, List<string> roles)
        {
            var configValue = AppsettHelper.GetValue("Jwt:SecretKey");
            LoggerHelper.Error($"SecretKey from config: {configValue}");

            // 获取 JWT 配置
            var jwtTokenModel = AppsettHelper.appSingle<JwtToken>("Jwt");

            // 设置用户信息
            jwtTokenModel.Uid = userId;
            jwtTokenModel.UserName = userName;

            // 生成 token
            var stringToken = CreateToken(jwtTokenModel, roles);
            return stringToken;
        }

        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="jwtTokenModel"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        private static string CreateToken(JwtToken jwtTokenModel, List<string> roles)
        {
            //创建 Claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwtTokenModel.Uid.ToString()), // 用户ID
                new Claim(JwtRegisteredClaimNames.Name, jwtTokenModel.UserName), // 用户名
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // JWT 唯一标识
            };

            //添加角色声明
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            //生成密钥
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenModel.SecretKey));
            var signingCredential = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            //创建 JWT
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtTokenModel.Issuer,
                audience: jwtTokenModel.Audience,
                expires: DateTime.Now.AddMinutes(jwtTokenModel.Expires),
                signingCredentials: signingCredential,
                claims: claims
            );

            //生成并返回 token 字符串
            var stringToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return stringToken;
        }
    }
}
