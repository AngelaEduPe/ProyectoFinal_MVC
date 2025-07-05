using CapaEntidades;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal_MVC.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioBL usuarioBL = new UsuarioBL();

        // GET: Usuario/RegistrarNuevoUsuario (nombre más genérico)
        public ActionResult AgregarNuevoUsuario()
        {
            Usuario usuarioActual = Session["Usuario"] as Usuario;

            if (usuarioActual == null || usuarioActual.idPerfil != 2)
            {
                return RedirectToAction("AccesoDenegado", "Error");
            }

            // Obtener la lista de perfiles para el DropDownList
            List<Perfil> perfiles = usuarioBL.ObtenerPerfiles();
            ViewBag.PerfilesList = new SelectList(perfiles, "idPerfil", "descripcion");

            return View();
        }

        // POST: Usuario/RegistrarNuevoUsuario
        [HttpPost]
        [ValidateAntiForgeryToken] // ¡No olvides este atributo!
        public JsonResult AgregarNuevoUsuario(Usuario nuevoUsuario, string contrasena, int idPerfilSeleccionado) // Nuevo parámetro
        {
            Usuario usuarioActual = Session["Usuario"] as Usuario;

            if (usuarioActual == null || usuarioActual.idPerfil != 2)
            {
                return Json(new { exito = false, mensaje = "Acceso denegado: solo un usuario con perfil de Jefatura puede agregar nuevos usuarios." });
            }

            if (ModelState.IsValid)
            {
                string baseUrlApp = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~");
                string rutaCarpetaPlantillasEmail = Server.MapPath("~/PlantillasEmail");

                string mensaje = "";
                try
                {
                    // **** CAMBIO CLAVE: Llamar al método genérico y pasar el idPerfilSeleccionado ****
                    mensaje = usuarioBL.AgregarNuevoUsuario(nuevoUsuario, contrasena, baseUrlApp, rutaCarpetaPlantillasEmail, usuarioActual.idUsuario, idPerfilSeleccionado);

                    if (mensaje.Contains("creado correctamente"))
                    {
                        return Json(new { exito = true, mensaje = mensaje, redirectUrl = Url.Action("ListaUsuarios", "Usuario") }); // Redirige a una lista de usuarios general
                    }
                    else
                    {
                        return Json(new { exito = false, mensaje = mensaje });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { exito = false, mensaje = "Ocurrió un error inesperado durante el registro: " + ex.Message });
                }
            }
            else
            {
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { exito = false, mensaje = "Datos de entrada inválidos.", errores = errores });
            }
        }
    }
}