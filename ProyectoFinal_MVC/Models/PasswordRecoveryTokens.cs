//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoFinal_MVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PasswordRecoveryTokens
    {
        public int IdToken { get; set; }
        public string Token { get; set; }
        public int IdUsuario { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public System.DateTime FechaExpiracion { get; set; }
        public Nullable<bool> Usado { get; set; }
    
        public virtual Usuario Usuario { get; set; }
    }
}
