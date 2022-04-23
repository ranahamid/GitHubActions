using System.Collections.Generic;

namespace WebApplication5.Data
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public int CountryCode { get; set; }
        public string ShortName { get; set; }
        public int GMTOffset { get; set; }
        public virtual IList<Hotel> Hotels { get; set; }
    }
}
