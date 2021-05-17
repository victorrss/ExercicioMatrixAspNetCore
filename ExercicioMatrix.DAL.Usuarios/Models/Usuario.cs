using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ExercicioMatrix.DAL.Usuarios
{
    public class Usuario : IdentityUser
    {
        public ICollection<IdentityRole> Roles { get; set; } = new List<IdentityRole>();
    }
}
