using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Areas.Identity.Data.User
{
    public class UserRegistration
    {
        [EmailAddress]
        public string Email { get; set;}
        public string Password { get; set; }
        public string Type { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }


    }
}
