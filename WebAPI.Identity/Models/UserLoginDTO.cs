using System.ComponentModel.DataAnnotations;

namespace WebAPI.Identity.Models
{
    public class UserLoginDTO
    {
        public string UserName { get; set; }
       
        public string Password { get; set; } 
        public string FullName { get; set; }
        
    }
}
