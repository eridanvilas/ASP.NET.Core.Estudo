using System.ComponentModel.DataAnnotations;

namespace WebAPI.Identity.Models
{
    public class UserDTO
    {
        public string UserName { get; set; }

        public string FullName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


    }
}
