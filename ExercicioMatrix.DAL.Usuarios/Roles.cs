using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExercicioMatrix.DAL.Usuarios
{
    public static class Roles
    {
        public const string ROLE_CREATE = "CR";
        public const string ROLE_READ = "RD";
        public const string ROLE_UPDATE = "UP";
        public const string ROLE_DELETE = "DL";
        public const string ROLE_ADMIN = "AD";
        public const string CLAIM_NAME = "Role";
        public static List<string> RolesExistentes()
        {
            return new List<string>
            {
                ROLE_ADMIN,
                ROLE_CREATE,
                ROLE_READ,
                ROLE_UPDATE,
                ROLE_DELETE
            };
        }
    }
}
