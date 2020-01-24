using Microsoft.AspNetCore.Identity;

namespace WebApp.Exercicio
{
    public class MyUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Member { get; set; } = "Member";
        public string OrgId { get; set; }
       
    }

    public class Organization
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
