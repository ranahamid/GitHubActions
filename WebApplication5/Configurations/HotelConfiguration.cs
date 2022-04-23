using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication5.Data;
namespace WebApplication5.Configurations
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
        
            builder.HasData(
                new Hotel { Id =1, Address = "Dhaka", CountryId = 1, Name = "Dhaka Regency", Rating = 4.9},
                new Hotel { Id = 2, Address = "Kurigram", CountryId = 2, Name = "Bhunga AK", Rating = 4.1 },
                new Hotel { Id = 3, Address = "Dhaka 2", CountryId = 1, Name = "Dhaka Regency", Rating = 4.9 },
                new Hotel { Id = 4, Address = "Kurigram 2", CountryId = 2, Name = "Bhunga AK", Rating = 4.1 }
            );
        }
    }
}
