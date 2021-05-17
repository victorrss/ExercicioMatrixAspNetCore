using AutoMapper;
using ExercicioMatrix.DAL.Usuarios;
using ExercicioMatrix.DAL.Usuarios.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace ExercicioMatrixAspNetCore.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Usuario, UsuarioApiResponse>()
                .ForMember(dest => dest.Permissoes, source => source.MapFrom(source => source.Roles.Select(p => p.Name).ToArray()));

            CreateMap<UsuarioAddUpdateRequest, Usuario>();
            //.ForMember(dest => dest.Roles, source => source.MapFrom(source => source.Permissoes.Select(p => new IdentityRole { Name = p }).ToArray()));
        }
    }
}
