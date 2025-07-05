using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class PasswordRecoveryTokenBL
    {
        private readonly PasswordRecoveryDAL oPasswordRecoveryDAL = new PasswordRecoveryDAL();

        public void GuardarTokenRecuperacion(int idUsuario, string token, DateTime fechaExpiracion)
        {
            oPasswordRecoveryDAL.GuardarTokenRecuperacion(idUsuario, token, fechaExpiracion);
        }

        public PasswordRecoveryToken ObtenerTokenValido(string token)
        {
            return oPasswordRecoveryDAL.ObtenerTokenValido(token);
        }

        public void InvalidarToken(string token)
        {
            oPasswordRecoveryDAL.InvalidarToken(token);
        }
    }
}
