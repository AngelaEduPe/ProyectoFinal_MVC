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
        private LoginBL loginBL = new LoginBL();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult Autenticar(string usuario, string contrasena)
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