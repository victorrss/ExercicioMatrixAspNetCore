using Microsoft.AspNetCore.Identity;
using System;

namespace ExercicioMatrix.DAL.Usuarios
{
    public class IdentityRoleInitializer
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityRoleInitializer(
            AuthDbContext context,
            UserManager<Usuario> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            if (_context.Database.EnsureCreated())
            {
                string[] roles =
            {
                    Roles.ROLE_ADMIN,
                    Roles.ROLE_CREATE,
                    Roles.ROLE_READ,
                    Roles.ROLE_UPDATE,
                    Roles.ROLE_DELETE
                };

                // Adiciona as roles
                foreach (var role in roles)
                {
                    if (!_roleManager.RoleExistsAsync(role).Result)
                    {
                        var resultado = _roleManager.CreateAsync(
                            new IdentityRole(role)).Result;
                        if (!resultado.Succeeded)
                        {
                            throw new Exception(
                                $"Erro durante a criação da role { role }.");
                        }
                    }
                }

                CreateUser(
                    new Usuario()
                    {
                        UserName = "admin",
                        Email = "admin@exerciciomatrixaspnetcore.com",
                    }, "123456", Roles.ROLE_ADMIN);

                CreateUser(
                    new Usuario()
                    {
                        UserName = "usr-create",
                        Email = "usr-create@exerciciomatrixaspnetcore.com",
                        EmailConfirmed = true
                    }, "123456", Roles.ROLE_CREATE);
            }
        }

        private void CreateUser(
            Usuario user,
            string password,
            string initialRole = null)
        {
            if (_userManager.FindByNameAsync(user.UserName).Result == null)
            {
                var resultado = _userManager
                    .CreateAsync(user, password).Result;

                if (resultado.Succeeded &&
                    !String.IsNullOrWhiteSpace(initialRole))
                {
                    _userManager.AddToRoleAsync(user, initialRole).Wait();
                }
            }
        }
    }
}
