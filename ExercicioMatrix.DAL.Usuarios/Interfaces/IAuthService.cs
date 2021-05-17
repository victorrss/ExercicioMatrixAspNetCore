using ExercicioMatrix.DAL.Usuarios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExercicioMatrix.DAL.Usuarios.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginModel model);
    }
}
