using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class PasswordRecoveryToken
    {
        public int IdToken { get; set; }
        public string Token { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool Usado { get; set; }
    }
}