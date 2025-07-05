using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class PerfilDAL
    {
        // Cadena de conexión a la base de datos, obtenida del archivo Web.config o App.config
        private readonly string cadena = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;

        /// <summary>
        /// Obtiene una lista de todos los perfiles de usuario desde la base de datos.
        /// </summary>
        /// <returns>Una lista de objetos Perfil.</returns>
        public List<Perfil> ObtenerTodosLosPerfiles()
        {
            List<Perfil> listaPerfiles = new List<Perfil>();

            // Abre una nueva conexión SQL usando la cadena de conexión
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                // Crea un comando SQL para ejecutar el procedimiento almacenado
                SqlCommand cmd = new SqlCommand("sp_ObtenerTodosLosPerfiles", cn);
                cmd.CommandType = CommandType.StoredProcedure; // Indica que el comando es un procedimiento almacenado

                try
                {
                    cn.Open(); // Abre la conexión a la base de datos
                    SqlDataReader reader = cmd.ExecuteReader(); // Ejecuta el comando y obtiene un lector de datos

                    // Itera sobre los resultados del lector para poblar la lista de perfiles
                    while (reader.Read())
                    {
                        listaPerfiles.Add(new Perfil
                        {
                            idPerfil = Convert.ToInt32(reader["IdPerfil"]), // Lee el IdPerfil
                            descripcion = reader["Descripcion"].ToString() // Lee la Descripcion del perfil (tu nombre de columna)
                        });
                    }
                    reader.Close(); // Cierra el lector de datos
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones: Lanza una nueva excepción con un mensaje descriptivo
                    // En una aplicación real, también deberías registrar este error.
                    throw new Exception("Error al obtener los perfiles: " + ex.Message, ex);
                }
            }
            return listaPerfiles;
        }
    }
}
