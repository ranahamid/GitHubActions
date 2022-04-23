using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication5.Models
{
    public class CountryDto: UpdateCountryDto
    {
     
        public virtual IList<HotelDto> Hotels { get; set; }
    }

    public class UpdateCountryDto: CreateCountryDto
    {
        public int Id { get; set; }
    }
    public class CreateCountryDto
    { 
        [Required]
        [StringLength(100, ErrorMessage = "Country Name is too long.")]
        public string Name { get; set; }
        [Required]
        [StringLength(3, ErrorMessage = "Country Short Name is too long.")]
        public string ShortName { get; set; }
    }
}
