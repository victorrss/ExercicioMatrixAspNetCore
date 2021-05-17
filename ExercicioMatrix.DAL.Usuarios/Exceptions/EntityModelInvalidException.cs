using System;

namespace ExercicioMatrix.DAL.Usuarios.Exceptions
{
    public class EntityModelInvalidException : Exception
    {
        public EntityModelInvalidException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }
    }
}