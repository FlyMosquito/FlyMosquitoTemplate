#region using
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FlyMosquito.Domain;
#endregion

namespace FlyMosquito.Core.EntityTypeConfig
{
    public class WebApiLogTypeConfig : IEntityTypeConfiguration<WebApiLog>
    {
        public void Configure(EntityTypeBuilder<WebApiLog> builder)
        {
            builder.ToTable("WebApiLog");

            builder.HasKey(e => e.Id).HasName("PK_WebApiLog");

            builder.Property(e => e.Controller)
               .HasMaxLength(50)
               .IsUnicode(false)
               .HasColumnName("Controller")
               .HasComment("控制器名称");

            builder.Property(e => e.Action)
               .HasMaxLength(50)
               .IsUnicode(false)
               .HasColumnName("Action")
               .HasComment("行为");

            builder.Property(e => e.Request)
                .HasMaxLength(2000)
                .HasColumnName("Request")
                .HasComment("请求");

            builder.Property(e => e.Response)
                .HasMaxLength(2000)
                .HasColumnName("Response")
                .HasComment("响应");

            builder.Property(e => e.CreateTime)
                .HasColumnType("datetime")
                .HasColumnName("CreateTime")
                .HasComment("创建时间");

            builder.Property(e => e.LoginAccount)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LoginAccount")
                .HasComment("登录账号");

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
