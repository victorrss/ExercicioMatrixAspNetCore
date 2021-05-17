using AutoMapper;
using ExercicioMatrix.DAL.Usuarios.Exceptions;
using ExercicioMatrix.DAL.Usuarios.Interfaces;
using ExercicioMatrix.DAL.Usuarios.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExercicioMatrix.DAL.Usuarios.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IMapper _mapper;

        public UsuarioService(IMapper mapper, UserManager<Usuario> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<string> AdicionarAsync(UsuarioAddUpdateRequest req)
        {
            // Valida se as permissoes informadas existem no sistema
            if (req.Permissoes != null)
                foreach (var permissao in req.Permissoes)
                {
                    if (String.IsNullOrWhiteSpace(permissao))
                        throw new EntityModelInvalidException($"Permissão {permissao} inválida");

                    if (!Roles.RolesExistentes().Contains(permissao.ToUpper()))
                        throw new EntityNotFoundException($"Permissão {permissao} inexistente");
                }

            var usuario = _mapper.Map<Usuario>(req);

            if (_userManager.FindByNameAsync(usuario.UserName).Result == null)
            {
                var resultado = _userManager
                    .CreateAsync(usuario, req.Password).Result;

                if (resultado.Succeeded)
                {
                    foreach (var permissao in req.Permissoes)
                    {
                        await _userManager.AddToRoleAsync(usuario, permissao.ToUpper());
                    }
                }
                else
                {
                    string sMsgErro = string.Empty;
                    foreach (var err in resultado.Errors)
                        sMsgErro += err.Description + "\r\n";

                    throw new EntityModelInvalidException($"Não foi possível criar o usuário.\r\nDetalhes: {sMsgErro}");
                }
            }
            else
                throw new EntityUniqueViolatedException("Usuario existente");

            return usuario.Id;
        }

        public async Task EditarAsync(UsuarioAddUpdateRequest req)
        {
            // Valida se as permissoes informadas existem no sistema
            foreach (var permissao in req.Permissoes)
            {
                if (String.IsNullOrWhiteSpace(permissao))
                    throw new EntityModelInvalidException($"Permissão {permissao} inválida");

                if (!Roles.RolesExistentes().Contains(permissao.ToUpper()))
                    throw new EntityNotFoundException($"Permissão {permissao} inexistente");
            }

            Usuario usuario = await _userManager.FindByNameAsync(req.UserName);

            if (usuario == null)
                throw new EntityNotFoundException("Usuario não encontrado");

            usuario.Email = req.Email;
            usuario.Roles = null;
            await _userManager.UpdateAsync(usuario);

            // Remove todas as permissoes e adiciona as que vierem da requisicao
            if (req.Permissoes.Length > 0)
            {
                var resultRoles = await _userManager.GetRolesAsync(usuario);
                await _userManager.RemoveFromRolesAsync(usuario, resultRoles);
                
                foreach (var permissao in req.Permissoes)
                    await _userManager.AddToRoleAsync(usuario, permissao.ToUpper());
            }

            // Troca de senha
            if (!string.IsNullOrWhiteSpace(req.Password))
            {
                await _userManager.RemovePasswordAsync(usuario);
                await _userManager.AddPasswordAsync(usuario, req.Password);
            }
        }

        public async Task RemoverAsync(UsuarioRemoveGetByIdRequest req)
        {
            var model = await _userManager.FindByIdAsync(req.Id);
            if (model == null)
            {
                throw new EntityNotFoundException("Usuario não encontrado");
            }
            await _userManager.DeleteAsync(model);
        }

        public async Task<Usuario> ListarUsuarioPorIdAsync(UsuarioRemoveGetByIdRequest req)
        {
            var usuario = await _userManager.FindByIdAsync(req.Id);
            if (usuario != null)
            {
                usuario.Roles = (await _userManager.GetRolesAsync(usuario)).Select(r => new IdentityRole { Name = r }).ToList();
            }
            return usuario;
        }

        public async Task<Usuario> ListarUsuarioPorNomeAsync(UsuarioGetByNameRequest req)
        {
            var usuario = await _userManager.FindByNameAsync(req.UserName);
            if (usuario != null)
            {
                usuario.Roles = (await _userManager.GetRolesAsync(usuario)).Select(r => new IdentityRole { Name = r }).ToList();
            }
            return usuario;
        }

        public async Task<IEnumerable<UsuarioApiResponse>> ListarTodosAsync()
        {
            List<Usuario> usuarios = _userManager.Users.ToList();
            foreach (var usuario in usuarios)
                usuario.Roles = (await _userManager.GetRolesAsync(usuario)).Select(r => new IdentityRole { Name = r }).ToList();

            return usuarios.Select(u => _mapper.Map<UsuarioApiResponse>(u));
        }
    }
}