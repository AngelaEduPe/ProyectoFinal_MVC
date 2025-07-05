using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal_MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginBL loginBL = new LoginBL();

        // GET: Login/Index (O la página principal de tu aplicación)
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult IniciarSesion()
        {
            return View();
        }


        [HttpPost]

        public JsonResult VerificarCredenciales(string usuario, string contrasena)
        {

            var user = loginBL.VerificarLogin(usuario, contrasena);
            if (user != null)
            {
                Session["Usuario"] = user; 
                return Json(new
                {
                    exito = true,
                    mensaje = "Login correcto",
                    nombre = user.nombre,
                    perfil = user.idPerfil 
                });
            }
            else
            {
                return Json(new
                {
                    exito = false,
                    mensaje = "Usuario o contraseña incorrectos"
                });
            }
        }
    }
}