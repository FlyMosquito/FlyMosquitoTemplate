#region using
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FlyMosquito.Domain;
#endregion

namespace FlyMosquito.Core
{
    public class UserTypeConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(e => e.Id).HasName("PK_User");

            builder.Property(e => e.Uid)
              .HasMaxLength(64)
              .IsUnicode(false)
              .HasColumnName("Uid")
              .HasComment("登陆账号");

            builder.Property(e => e.UserName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("UserName")
                .HasComment("用户姓名");

            builder.Property(e => e.PassWord)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("PassWord")
                .HasComment("用户密码");

            //设置 IsEnable 字段默认值为 0
            builder.Property(e => e.IsEnable)
                .HasDefaultValue(0)  //默认值为 0
                .HasColumnName("IsEnable")
                .HasComment("是否启用 0可用（默认） 1禁用");
        }
    }
}
