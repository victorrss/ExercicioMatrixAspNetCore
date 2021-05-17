using ExercicioMatrix.DAL.Usuarios.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExercicioMatrix.DAL.Usuarios.Interfaces
{
    public interface IUsuarioService
    {
        Task<string> AdicionarAsync(UsuarioAddUpdateRequest req);

        Task EditarAsync(UsuarioAddUpdateRequest req);

        Task RemoverAsync(UsuarioRemoveGetByIdRequest req);

        Task<Usuario> ListarUsuarioPorIdAsync(UsuarioRemoveGetByIdRequest req);

        Task<Usuario> ListarUsuarioPorNomeAsync(UsuarioGetByNameRequest req);

        Task<IEnumerable<UsuarioApiResponse>> ListarTodosAsync();
    }
}
