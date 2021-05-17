namespace ExercicioMatrix.DAL.Usuarios.Models
{
    public class UsuarioAddUpdateRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string[] Permissoes { get; set; } = { };
    }
}