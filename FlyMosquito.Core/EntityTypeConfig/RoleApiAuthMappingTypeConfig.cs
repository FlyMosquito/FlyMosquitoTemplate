#region using
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FlyMosquito.Domain;
#endregion

namespace FlyMosquito.Core
{
    public class RoleApiAuthMappingTypeConfig : IEntityTypeConfiguration<RoleApiAuthMapping>
    {
        public void Configure(EntityTypeBuilder<RoleApiAuthMapping> builder)
        {
            builder.ToTable("RoleApiAuthMapping");

            builder.HasKey(e => e.Id).HasName("PK_RoleApiAuthMapping");

            builder.Property(e => e.RoleId)
               .IsUnicode(false)
               .HasColumnName("RoleId")
               .HasComment("角色ID");

            builder.Property(e => e.AuthId)
               .IsUnicode(false)
               .HasColumnName("AuthId")
               .HasComment("路由ID");
        }
    }
}
