using ExercicioMatrix.DAL.Usuarios.Interfaces;
using ExercicioMatrix.DAL.Usuarios.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExercicioMatrix.DAL.Usuarios.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IUsuarioService _usuarioService;

        public AuthService(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IUsuarioService usuarioService)
        {
            this._signInManager = signInManager;
            this._usuarioService = usuarioService;
        }

        public async Task<string> LoginAsync(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, true, true);
            if (result.Succeeded)
            {
                Usuario usuario = await _usuarioService.ListarUsuarioPorNomeAsync(new UsuarioGetByNameRequest { UserName = model.Login });
                return CriarToken(usuario);
            }
            return null;
        }

        private static string CriarToken(Usuario model)
        {
            // Criar o token (header + payload(direitos) + signature)
            List<Claim> direitos = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, model.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Adiciona as permissões no token jwt
            if (model.Roles != null)
                foreach (var permissao in model.Roles)
                {
                    direitos.Add(new Claim(Roles.CLAIM_NAME, permissao.Name));
                }

            var chave = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("auth-exercicio-matrix"));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "ExericioMatrixAspNetCore",
                audience: "Insomnia",
                claims: direitos,
                signingCredentials: credenciais,
                expires: DateTime.Now.AddMinutes(5)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }
    }
}