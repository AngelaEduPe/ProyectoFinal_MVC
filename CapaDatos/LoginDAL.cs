using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidades;
using System.Configuration;

namespace CapaDatos
{
    public class LoginDAL
    {

        private readonly string cadena = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;

        public Usuario IniciarSesion(string usuarioInput, string contrasenaInput)
        {
            Usuario usuarioEncontrado = null;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand();

                try
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_LoginUsuarios";
                    cmd.Parameters.AddWithValue("@Usuario", usuarioInput);
                    cmd.Parameters.AddWithValue("@Contrasena", contrasenaInput);

                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        usuarioEncontrado = new Usuario
                        {
                            idUsuario = Convert.ToInt32(reader["IdUsuario"]),
                            usuario = reader["Usuario"].ToString(),
                            nombre = reader["Nombre"].ToString(),
                            apellidoPaterno = reader["ApellidoPaterno"].ToString(),
                            apellidoMaterno = reader["ApellidoMaterno"].ToString(),
                            idPerfil = Convert.ToInt32(reader["IdPerfil"]),
                        };
                    }

                    reader.Close();
                    cn.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al iniciar sesión: " + ex.Message);
                }
            }

            return usuarioEncontrado;
        }


        public bool VerificarContrasenaUsuario(int idUsuario, string contrasenaPlano)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_VerificarContrasenaUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@ContrasenaPlano", contrasenaPlano);

                try
                {
                    cn.Open();
                    int resultado = (int)cmd.ExecuteScalar();
                    return resultado == 1;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al verificar la contraseña anterior: " + ex.Message);
                }
            }
        }

    }
}
