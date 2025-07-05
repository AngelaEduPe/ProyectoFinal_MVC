using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class Docente
    {
        public int idDocente { get; set; }
        public int idUsuario { get; set; }
        public int idTipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public int idGenero { get; set; }
        public int idEstadoCivil { get; set; }
        public string direccion { get; set; }
        public string idDepartamento { get; set; }
        public string idProvincia { get; set; } 
        public string idDistrito { get; set; } 
        public string telefono { get; set; }
        public string celular { get; set; }
        public byte[] foto { get; set; }
        public decimal costoHora { get; set; }

        public int? idUsuarioCreacion { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public int? idUsuarioModificacion { get; set; }
        public DateTime? fechaModificacion { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string usuario {  get; set; }
        public string nombreTipoDocumento { get; set; }
        public string genero { get; set; }
        public string estadoCivil { get; set; }
        public string departamento { get; set; }
        public string provincia { get; set; }
        public string distrito { get; set; }

        public Docente()
        {
            
        }

        public Docente(int idUsuario, int idTipoDocumento, string numeroDocumento, int idGenero, int idEstadoCivil,
               string direccion, string idDepartamento, string idProvincia, string idDistrito,
               string telefono, string celular, byte[] foto, decimal costoHora,
               int? idUsuarioCreacion = null, DateTime? fechaCreacion = null,
               int? idUsuarioModificacion = null, DateTime? fechaModificacion = null)
        {
            this.idUsuario = idUsuario;
            this.idTipoDocumento = idTipoDocumento;
            this.numeroDocumento = numeroDocumento;
            this.idGenero = idGenero;
            this.idEstadoCivil = idEstadoCivil;
            this.direccion = direccion;
            this.idDepartamento = idDepartamento;
            this.idProvincia = idProvincia;
            this.idDistrito = idDistrito;
            this.telefono = telefono;
            this.celular = celular;
            this.foto = foto;
            this.costoHora = costoHora;
            this.idUsuarioCreacion = idUsuarioCreacion;
            this.fechaCreacion = fechaCreacion;
            this.idUsuarioModificacion = idUsuarioModificacion;
            this.fechaModificacion = fechaModificacion;
        }
    }
}
