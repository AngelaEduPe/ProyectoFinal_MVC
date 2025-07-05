using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicios
{
    public class EmailService
    {

        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly bool _enableSsl;

        public EmailService()
        {

            _smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            _smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            _smtpUser = ConfigurationManager.AppSettings["SmtpUser"];
            _smtpPass = ConfigurationManager.AppSettings["SmtpPass"];
            _enableSsl = bool.Parse(ConfigurationManager.AppSettings["EnableSsl"]);
        }


        public void EnviarEmail(string destinatario, string asunto, string cuerpoHtml)
        {
            try
            {

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(_smtpUser, "Plataforma de Gestión Docente");
                    mail.To.Add(destinatario); 
                    mail.Subject = asunto; 
                    mail.Body = cuerpoHtml; 
                    mail.IsBodyHtml = true; 
                    mail.Priority = MailPriority.High;

                   
                    using (SmtpClient smtp = new SmtpClient(_smtpServer, _smtpPort))
                    {
                        smtp.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                        smtp.EnableSsl = _enableSsl;
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo enviar el correo de confirmación: " + ex.Message);
            }
        }


        public string ObtenerCuerpoHtml(string nombrePlantilla, Dictionary<string, string> sustituciones, string rutaCarpetaPlantillas)
        {
            string rutaPlantilla = Path.Combine(rutaCarpetaPlantillas, nombrePlantilla + ".html");

            if (!File.Exists(rutaPlantilla))
            {
                throw new FileNotFoundException($"Plantilla de email no encontrada: {rutaPlantilla}");
            }

            string contenidoHtml = File.ReadAllText(rutaPlantilla);

            foreach (var sustitucion in sustituciones)
            {
                contenidoHtml = contenidoHtml.Replace("{{" + sustitucion.Key + "}}", sustitucion.Value);
            }

            return contenidoHtml;
        }

        public string EnviarCorreoConfirmacionRegistro(Usuario unUsuario, string baseUrlApp, string rutaCarpetaPlantillasEmail)
        {
            string mensaje = "";
            string asunto = "¡Registro Exitoso en la Plataforma de Gestión de Docentes!";
            string urlLogin = $"{baseUrlApp}/InicioSesion.aspx";

            Dictionary<string, string> sustituciones = new Dictionary<string, string>
            {
                { "nombreUsuario", $"{unUsuario.nombre} {unUsuario.apellidoPaterno}" },
                { "correoUsuario", unUsuario.usuario },
                { "urlLogin", urlLogin }
            };

            string cuerpoHtml = ObtenerCuerpoHtml("RegistroExitoso", sustituciones, rutaCarpetaPlantillasEmail);

            try
            {
                EnviarEmail(unUsuario.usuario, asunto, cuerpoHtml);
                mensaje = "Usuario creado correctamente.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar correo de confirmación a {unUsuario.usuario}: {ex.Message}");
                mensaje = "Usuario creado correctamente (Advertencia: No se pudo enviar el correo de confirmación. Por favor, revise su dirección de correo o contacte a soporte).";
            }
            return mensaje;
        }


        public bool EnviarCorreoSolicitudRecuperacion(Usuario usuario, string linkRecuperado, string rutaCarpetaPlantillasEmail)
        {
            string asunto = "Restablecimiento de Contraseña - Plataforma de Gestión Docente";

            Dictionary<string, string> sustituciones = new Dictionary<string, string>
            {
                { "nombreUsuario", $"{usuario.nombre} {usuario.apellidoPaterno}" },
                { "linkRecuperado", linkRecuperado }
            };

            string cuerpoHtml = ObtenerCuerpoHtml("RecuperacionContrasena", sustituciones, rutaCarpetaPlantillasEmail);

            try
            {
                
                EnviarEmail(usuario.usuario, asunto, cuerpoHtml);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar correo de recuperación a {usuario.usuario}: {ex.Message}");
                
                return false;
            }
        }

        public void EnviarCorreoNotificacionContrasena(Usuario usuario, string rutaCarpetaPlantillasEmail)
        {
            string asunto = "Su contraseña ha sido actualizada"; 

            Dictionary<string, string> sustituciones = new Dictionary<string, string>
            {
                { "nombreUsuario", $"{usuario.nombre} {usuario.apellidoPaterno}" }
            };
            string cuerpoHtml = ObtenerCuerpoHtml("ContrasenaRestablecida", sustituciones, rutaCarpetaPlantillasEmail);

            try
            {
                EnviarEmail(usuario.usuario, asunto, cuerpoHtml);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar correo de notificación de contraseña a {usuario.usuario}: {ex.Message}");
            }
        }


    }
}