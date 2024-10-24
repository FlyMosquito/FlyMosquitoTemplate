#region using
using Microsoft.EntityFrameworkCore;
using FlyMosquito.Core.EntityTypeConfig;
using FlyMosquito.Domain;
using FlyMosquito.Domain.Basis;
#endregion

namespace FlyMosquito.Core
{
    public class FlyMosquitoDbContext : DbContext
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public FlyMosquitoDbContext()
        {
        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="options"></param>
        public FlyMosquitoDbContext(DbContextOptions<FlyMosquitoDbContext> options) : base(options)
        {
        }
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserRoleMapping> UserRoleMapping { get; set; }
        public DbSet<ApiAuth> ApiAuth { get; set; }
        public DbSet<RoleApiAuthMapping> RoleApiAuthMapping { get; set; }
        public DbSet<LoginLog> LoginLog { get; set; }
        public DbSet<WebApiLog> WebApiLog { get; set; }
        public DbSet<UserApiAuthMapping> UserApiAuthMapping { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=****;;database=****;uid=sa;pwd=****;;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RoleTypeConfig());
            modelBuilder.ApplyConfiguration(new UserRoleMappingTypeConfig());
            modelBuilder.ApplyConfiguration(new UserTypeConfig());

            modelBuilder.ApplyConfiguration(new ApiAuthTypeConfig());
            modelBuilder.ApplyConfiguration(new RoleApiAuthMappingTypeConfig());

            modelBuilder.ApplyConfiguration(new LoginLogEntityTypeConfig());
            modelBuilder.ApplyConfiguration(new WebApiLogTypeConfig());

            modelBuilder.ApplyConfiguration(new UserApiAuthMappingTypeConfig());
        }
    }
}
