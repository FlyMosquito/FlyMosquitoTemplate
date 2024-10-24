#region using
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FlyMosquito.Domain;
#endregion

namespace FlyMosquito.Core
{
    public class UserRoleMappingTypeConfig : IEntityTypeConfiguration<UserRoleMapping>
    {
        public void Configure(EntityTypeBuilder<UserRoleMapping> builder)
        {
            builder.ToTable("UserRoleMapping");

            builder.HasKey(e => e.Id).HasName("PK_UserRoleMapping");

            builder.Property(e => e.UserId)
               .IsUnicode(false)
               .HasColumnName("UserId")
               .HasComment("用户ID");

            builder.Property(e => e.RoleId)
                .IsUnicode(false)
                .HasColumnName("RoleId")
                .HasComment("角色ID");
        }
    }
}
