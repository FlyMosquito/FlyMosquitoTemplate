namespace FlyMosquito.Domain
{
    public class JwtToken
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 发行者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        ///  接受者
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 过期时间（分钟）
        /// </summary>
        public int Expires { get; set; }
    }
}
