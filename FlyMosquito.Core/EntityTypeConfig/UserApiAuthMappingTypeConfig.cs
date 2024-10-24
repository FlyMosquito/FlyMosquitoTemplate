using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FlyMosquito.Domain.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyMosquito.Core.EntityTypeConfig
{
    public class UserApiAuthMappingTypeConfig : IEntityTypeConfiguration<UserApiAuthMapping>
    {
        public void Configure(EntityTypeBuilder<UserApiAuthMapping> builder)
        {
            builder.ToTable("UserApiAuthMapping");

            builder.HasKey(e => e.Id).HasName("PK_UserApiAuthMapping");

            builder.Property(e => e.UserId)
               .HasColumnName("UserId")
               .HasComment("用户ID");

            builder.Property(e => e.ApiAuthId)
           .HasColumnName("ApiAuthId")
           .HasComment("控制器ID");

            builder.Property(e => e.IsAllowed)
           .HasColumnName("IsAllowed")
           .HasComment("是否允许");
        }
    }
}
