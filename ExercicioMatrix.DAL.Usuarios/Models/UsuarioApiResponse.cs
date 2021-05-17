namespace ExercicioMatrix.DAL.Usuarios.Models
{
    public class UsuarioApiResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string[] Permissoes { get; set; } = { };
    }
}