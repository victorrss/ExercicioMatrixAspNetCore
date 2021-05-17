using System;

namespace ExercicioMatrix.DAL.Usuarios.Exceptions
{
    /// <summary>
    /// Exception que representa chave duplicada
    /// </summary>
    public class EntityUniqueViolatedException : Exception
    {
        public EntityUniqueViolatedException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }
    }
}