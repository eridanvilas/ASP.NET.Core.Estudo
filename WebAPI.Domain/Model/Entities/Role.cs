using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WebAPI.Domain.Model.Entities
{
    public class Role : IdentityRole<int>
    {
        public List<UserRole> UserRoles { get; set; }
    }
}
