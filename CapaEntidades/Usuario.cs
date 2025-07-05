using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public string usuario { get; set; }
        public byte[] contrasenaHash { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string nombre { get; set; }
        public int idPerfil { get; set; }
        public int idEliminado { get; set; }
        public int? idUsuarioCreacion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public DateTime? fechaModificacion { get; set; }

        public Usuario() { }

        public Usuario(string usuario, byte[] contrasenaHash, string apellidoPaterno,
                       string apellidoMaterno, string nombre, int idPerfil,
                       int? idUsuarioCreacion, DateTime? fechaCreacion)
        {
            this.usuario = usuario;
            this.contrasenaHash = contrasenaHash;
            this.apellidoPaterno = apellidoPaterno;
            this.apellidoMaterno = apellidoMaterno;
            this.nombre = nombre;
            this.idPerfil = idPerfil;
            this.idEliminado = 0;
            this.idUsuarioCreacion = idUsuarioCreacion;
            this.fechaCreacion = fechaCreacion;
        }
    }
}
