using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication5.Data;

namespace WebApplication5.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
                new Country { Id = 1, ShortName = "AUS", Name = "Australia", CountryCode = 880, GMTOffset = 360 },
                new Country { Id = 2, ShortName = "PAK", Name = "Pakistan", CountryCode = 91, GMTOffset = 330 }
            );
        }
    }
}
