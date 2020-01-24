using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPI.Domain.Model.Entities;

namespace WebAPI.Repository.Repositories
{
    public class ContextRepository : IdentityDbContext<User, Role, int,
                                                       IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
                                                       IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ContextRepository(DbContextOptions<ContextRepository> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(userRole => {
                userRole.HasKey(x => new { x.UserId, x.RoleId });

                userRole.HasOne(x => x.Role)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(x => x.RoleId)
                        .IsRequired();


                userRole.HasOne(x => x.User)
                        .WithMany(r => r.UserRoles)
                        .HasForeignKey(x => x.UserId)
                        .IsRequired();
            });

            builder.Entity<Organization>(org =>
            {
                org.ToTable("Organizations");
                org.HasKey(x => x.Id);

                org.HasMany<User>()
                   .WithOne()
                   .HasForeignKey(x => x.OrgId)
                   .IsRequired(false);
            });
        }
    }
}
