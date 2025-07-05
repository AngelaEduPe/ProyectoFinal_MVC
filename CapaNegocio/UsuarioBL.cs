using CapaDatos;
using CapaEntidades;
using CapaNegocio.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class UsuarioBL
    {
        private readonly UsuarioDAL oUsuarioDAL = new UsuarioDAL();
        private readonly PerfilDAL oPerfilDAL = new PerfilDAL();
        private readonly EmailService servicioEmail = new EmailService();
        private readonly PasswordRecoveryTokenBL oPasswordRecoveryTokenBL = new PasswordRecoveryTokenBL();

        /*public string agregarUsuarioDocente(Usuario unUsuario, string contrasenaPlano, string baseUrlApp, string rutaCarpetaPlantillasEmail)
        {
            string mensaje = oUsuarioDAL.agregarLoginDocente(unUsuario, contrasenaPlano);

            if (mensaje.Contains("creado correctamente") || mensaje == "")
            {
                mensaje = servicioEmail.EnviarCorreoConfirmacionRegistro(unUsuario, baseUrlApp, rutaCarpetaPlantillasEmail);
            }

            return mensaje;
        }*/

        public string AgregarNuevoUsuario(Usuario unUsuario, string contrasenaPlano, string baseUrlApp, string rutaCarpetaPlantillasEmail, int idUsuarioQueRegistra, int idPerfilNuevoUsuario)
        {
            // Autorización: Solo permitir el registro si el usuario que intenta registrar tiene idPerfil = 2 (Jefatura)
            Usuario usuarioRegistrador = oUsuarioDAL.ObtenerUsuarioPorId(idUsuarioQueRegistra);

            if (usuarioRegistrador == null || usuarioRegistrador.idPerfil != 2)
            {
                return "Solo un usuario con perfil de Jefatura puede agregar nuevos usuarios.";
            }

            // Validar que el idPerfilNuevoUsuario sea válido (puedes agregar lógica de validación aquí)
            // Por ejemplo, que sea 1 (Administrativo), 2 (Jefatura, si se permite que Jefatura cree otros Jefatura), o 3 (Docente)
            if (idPerfilNuevoUsuario <= 0) // Ejemplo de validación básica
            {
                return "Perfil de usuario inválido.";
            }

            // **** CAMBIO CLAVE: Pasar el idPerfilNuevoUsuario al DAL ****
            string mensaje = oUsuarioDAL.AgregarNuevoUsuario(unUsuario, contrasenaPlano, idPerfilNuevoUsuario, idUsuarioQueRegistra);

            if (mensaje.Contains("creado correctamente") || mensaje == "")
            {
                // Si el registro fue exitoso, envía el correo de confirmación
                mensaje = servicioEmail.EnviarCorreoConfirmacionRegistro(unUsuario, baseUrlApp, rutaCarpetaPlantillasEmail);
            }

            return mensaje;
        }

        public List<Perfil> ObtenerPerfiles()
        {
            return oPerfilDAL.ObtenerTodosLosPerfiles();
        }



        public bool ObtenerTokenValido(string token)
        {
            return oPasswordRecoveryTokenBL.ObtenerTokenValido(token) != null;
        }

        public string SolicitarRecuperacion(string email, string baseUrlApp, string rutaCarpetaPlantillasEmail)
        {
            Usuario usuario = oUsuarioDAL.ObtenerUsuarioPorEmail(email);

            if (usuario == null)
            {
                return "Si su dirección de correo está registrada, recibirá un enlace de recuperación.";
            }

            string token = Guid.NewGuid().ToString();
            DateTime fechaExpiracion = DateTime.Now.AddMinutes(5);
            oPasswordRecoveryTokenBL.GuardarTokenRecuperacion(usuario.idUsuario, token, fechaExpiracion);

            string linkRecuperado = $"{baseUrlApp}/RestablecerContrasena.aspx?token={token}";
            bool correoEnviado = servicioEmail.EnviarCorreoSolicitudRecuperacion(usuario, linkRecuperado, rutaCarpetaPlantillasEmail);

            if (correoEnviado)
            {
                return "Se ha enviado un enlace para restablecer tu contraseña a tu correo electrónico.";
            }
            else
            {
                throw new Exception("No se pudo enviar el correo de recuperación. Por favor, inténtalo de nuevo más tarde.");
            }
        }

        public string RestablecerContrasena(string token, string nuevaContrasenaPlano, string rutaCarpetaPlantillasEmail, int? idUsuarioModificacion = null)
        {
            PasswordRecoveryToken tokenRecuperado = oPasswordRecoveryTokenBL.ObtenerTokenValido(token);

            if (tokenRecuperado == null)
            {
                return "El enlace de recuperación es inválido o ha expirado.";
            }

            int idUsuarioAfectado = tokenRecuperado.IdUsuario;
            if (idUsuarioModificacion == null)
            {
                idUsuarioModificacion = idUsuarioAfectado;
            }

            try
            {
                oUsuarioDAL.ActualizarContrasena(idUsuarioAfectado, nuevaContrasenaPlano, idUsuarioModificacion);
                oPasswordRecoveryTokenBL.InvalidarToken(token);

                Usuario usuario = oUsuarioDAL.ObtenerUsuarioPorId(idUsuarioAfectado);

                if (usuario != null)
                {
                    servicioEmail.EnviarCorreoNotificacionContrasena(usuario, rutaCarpetaPlantillasEmail);
                }
                return "Contraseña restablecida exitosamente.";
            }
            catch (Exception ex)
            {
                return $"Error al restablecer la contraseña: {ex.Message}";
            }
        }

        public Usuario ObtenerUsuarioPorId(int idUsuario)
        {
            return oUsuarioDAL.ObtenerUsuarioPorId(idUsuario);
        }

        public void ActualizarContrasenaDirecta(int idUsuario, string nuevaContrasenaPlano, string rutaCarpetaPlantillasEmail, int? idUsuarioModificacion)
        {
            oUsuarioDAL.ActualizarContrasena(idUsuario, nuevaContrasenaPlano, idUsuarioModificacion);

            Usuario usuario = oUsuarioDAL.ObtenerUsuarioPorId(idUsuario);

            if (usuario != null)
            {
                servicioEmail.EnviarCorreoNotificacionContrasena(usuario, rutaCarpetaPlantillasEmail);
            }
        }
    }
}
