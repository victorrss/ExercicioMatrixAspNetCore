using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExercicioMatrix.DAL.Usuarios
{
    public class AuthDbContext : IdentityDbContext<Usuario>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Usuario>()
              .HasMany(u => u.Roles)
              .WithMany("User")
              .UsingEntity<IdentityUserRole<string>>(
                  userRole => userRole.HasOne<IdentityRole>()
                      .WithMany()
                      .HasForeignKey(ur => ur.RoleId)
                      .IsRequired(),
                  userRole => userRole.HasOne<Usuario>()
                      .WithMany()
                      .HasForeignKey(ur => ur.UserId)
                      .IsRequired());
        }
    }
}
