using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace WebApp.Exercicio.Models
{
    public class PasswordValidation<TUser> : IPasswordValidator<TUser> where TUser : class
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            var username = await manager.GetUserNameAsync(user);
            if (username == password)
            {
                return IdentityResult.Failed(
                    new IdentityError { Description = "A senha nao pode ser igual ao usuario" }
                );
            }
            if (password.Contains("password"))
            {
                return IdentityResult.Failed(
                new IdentityError { Description = "A senha nao pode ser password" }
                );
            }

            return IdentityResult.Success;

        }
    }
}
