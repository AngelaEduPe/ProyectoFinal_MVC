using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class LoginBL
    {
        LoginDAL loginDAL = new LoginDAL();

        public Usuario VerificarLogin(string usuario, string contrasena)
        {
            return loginDAL.IniciarSesion(usuario, contrasena);
        }

        public bool VerificarContrasenaUsuario(int idUsuario, string contrasenaPlano)

        {
            return loginDAL.VerificarContrasenaUsuario(idUsuario, contrasenaPlano);
        }

    }
}
