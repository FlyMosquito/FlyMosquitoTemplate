#region using
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FlyMosquito.Domain;
#endregion

namespace FlyMosquito.Core.EntityTypeConfig
{
    public class LoginLogEntityTypeConfig : IEntityTypeConfiguration<LoginLog>
    {
        public void Configure(EntityTypeBuilder<LoginLog> builder)
        {
            builder.ToTable("LoginLog");

            builder.HasKey(e => e.Id).HasName("PK_LoginLog");

            builder.Property(e => e.Token)
                .HasMaxLength(2000)
                .HasColumnName("Token")
                .HasComment("Token");

            builder.Property(e => e.LoginTime)
                .HasColumnType("datetime")
                .HasColumnName("LoginTime")
                .HasComment("登录时间");

            builder.Property(e => e.ExitingTime)
                .HasColumnType("datetime")
                .HasColumnName("ExitingTime")
                .HasComment("退出时间");

            builder.Property(e => e.LoginAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LoginAccount")
                .HasComment("登录账号");

            builder.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserName")
                .HasComment("用户名");

            builder.Property(e => e.LoginIP)
             .HasMaxLength(50)
             .HasColumnName("LoginIP")
             .HasComment("登录IP");

            builder.Property(e => e.LoginLocation)
             .HasMaxLength(50)
             .HasColumnName("LoginLocation")
             .HasComment("登录地址");
        }
    }
}
