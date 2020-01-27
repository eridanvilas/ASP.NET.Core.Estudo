namespace WebAPI.Identity.Models
{
    public class UpdateUserRoleDTO
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public bool Delete { get; set; }
    }
}
