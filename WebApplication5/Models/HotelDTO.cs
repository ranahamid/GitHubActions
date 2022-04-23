using System.ComponentModel.DataAnnotations;
using WebApplication5.Data;

namespace WebApplication5.Models
{

    public class HotelDto : UpdateHotelDto
    { 
        public CountryDto Country { get; set; }
    }
    public class UpdateHotelDto : CreateHotelDto
    {
        public int Id { get; set; }
    }
    public class CreateHotelDto
    {

        [Required]
        [StringLength(255, ErrorMessage = "Country Name is too long.")]
        public string Name { get; set; }
        [Required]
        [StringLength(255, ErrorMessage = "Address is too long.")]
        public string Address { get; set; }
        [Required]
        [Range(0, 5)]
        public double Rating { get; set; }
        [Required]
        public int CountryId { get; set; }
    }
}
