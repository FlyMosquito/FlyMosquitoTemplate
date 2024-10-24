#region using
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FlyMosquito.Domain;
#endregion

namespace FlyMosquito.Core
{
    public class RoleTypeConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");

            builder.HasKey(e => e.Id).HasName("PK_Role");

            builder.Property(e => e.RoleName)
               .HasMaxLength(64)
               .IsUnicode(false)
               .HasColumnName("RoleName")
               .HasComment("角色姓名");
        }
    }
}
