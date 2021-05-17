using System;

namespace ExercicioMatrix.DAL.Usuarios.Exceptions
{
    /// <summary>
    /// Exception que representa a entidade esperada não encontrada
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }
    }
}