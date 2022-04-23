using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication5.Configurations
{
    public class RoleConfiguration:IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Name="User",
                    NormalizedName = "User"
                },
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin"
                }
            );
        }
    }
}
