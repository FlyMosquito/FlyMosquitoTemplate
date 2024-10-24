#region using
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FlyMosquito.Domain;
#endregion

namespace FlyMosquito.Core
{
    public class ApiAuthTypeConfig : IEntityTypeConfiguration<ApiAuth>
    {
        public void Configure(EntityTypeBuilder<ApiAuth> builder)
        {
            builder.ToTable("ApiAuth");

            builder.HasKey(e => e.Id).HasName("PK_ApiAuth");

            builder.Property(e => e.AuthName)
               .HasMaxLength(64)
               .IsUnicode(false)
               .HasColumnName("AuthName")
               .HasComment("路由名称");

            builder.Property(e => e.RoutePath)
               .HasMaxLength(64)
               .IsUnicode(false)
               .HasColumnName("RoutePath")
               .HasComment("路径");

            builder.Property(e => e.Controller)
               .HasMaxLength(64)
               .IsUnicode(false)
               .HasColumnName("Controller")
               .HasComment("控制器");

            builder.Property(e => e.Action)
              .HasMaxLength(64)
              .IsUnicode(false)
              .HasColumnName("Action")
              .HasComment("方法");

            builder.Property(e => e.ActionDescription)
              .HasMaxLength(64)
              .IsUnicode(false)
              .HasColumnName("ActionDescription")
              .HasComment("描述");

        }
    }
}
