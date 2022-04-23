using Microsoft.AspNetCore.Identity;

namespace WebApplication5.Data
{
    public class ApiUser:IdentityUser
    {
        public string FirstName { get;set; }
        public string LastName { get; set; }
    }
}
