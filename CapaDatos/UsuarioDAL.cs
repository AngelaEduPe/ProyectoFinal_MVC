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
    public class UsuarioDAL
    {
        private readonly string cadena = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;

        /*public string agregarLoginDocente(Usuario unUsuario, string contrasenaPlano)
        {
            string r = "";

            if (string.IsNullOrEmpty(contrasenaPlano) ||
                contrasenaPlano.Length < 8 ||
                !contrasenaPlano.Any(char.IsUpper) ||
                !contrasenaPlano.Any(char.IsDigit) ||
                !contrasenaPlano.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                throw new Exception("La contraseña debe tener al menos 8 caracteres, incluir una mayúscula, un número y un carácter especial.");
            }

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_RegistrarUsuarioDocente";

                    cmd.Parameters.AddWithValue("@CorreoElectronico", unUsuario.usuario);
                    cmd.Parameters.AddWithValue("@Contrasena", contrasenaPlano);
                    cmd.Parameters.AddWithValue("@ApellidoPaterno", unUsuario.apellidoPaterno);
                    cmd.Parameters.AddWithValue("@ApellidoMaterno", unUsuario.apellidoMaterno);
                    cmd.Parameters.AddWithValue("@Nombre", unUsuario.nombre);

                    cn.Open();
                    int f = cmd.ExecuteNonQuery();
                    if (f > 0)
                    {
                        r = "Usuario creado correctamente.";
                    }
                    cn.Close();
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 50000)
                        r = ex.Message;
                    else
                        throw new Exception("Error SQL: " + ex.Message);
                }
                finally
                {
                    cn.Dispose();
                }
            }

            return r;
        }*/

        public string AgregarNuevoUsuario(Usuario unUsuario, string contrasenaPlano, int idPerfil, int? idUsuarioCreacion)
        {
            string r = "";

            if (string.IsNullOrEmpty(contrasenaPlano) ||
                contrasenaPlano.Length < 8 ||
                !contrasenaPlano.Any(char.IsUpper) ||
                !contrasenaPlano.Any(char.IsDigit) ||
                !contrasenaPlano.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                // Deberías usar una forma consistente de manejar estas excepciones o mensajes
                throw new Exception("La contraseña debe tener al menos 8 caracteres, incluir una mayúscula, un número y un carácter especial.");
            }

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_RegistrarUsuarioxJefatura";

                    cmd.Parameters.AddWithValue("@CorreoElectronico", unUsuario.usuario);
                    cmd.Parameters.AddWithValue("@Contrasena", contrasenaPlano);
                    cmd.Parameters.AddWithValue("@ApellidoPaterno", unUsuario.apellidoPaterno);
                    cmd.Parameters.AddWithValue("@ApellidoMaterno", unUsuario.apellidoMaterno);
                    cmd.Parameters.AddWithValue("@Nombre", unUsuario.nombre);
                    cmd.Parameters.AddWithValue("@IdPerfil", idPerfil);
                    cmd.Parameters.AddWithValue("@IdUsuarioCreacion", idUsuarioCreacion ?? (object)DBNull.Value);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                    r = "Usuario registrado correctamente.";
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 50000)
                    {
                        r = ex.Message;
                    }
                    else
                    {
                        r = "Error de base de datos al registrar usuario: " + ex.Message;
                    }
                }
                catch (Exception ex)
                {
                    r = "Ocurrió un error inesperado durante el registro: " + ex.Message;
                }
            }
            return r;
        }



        public Usuario ObtenerUsuarioPorEmail(string email)
        {
            Usuario usuario = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerUsuarioPorEmail", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Email", email);

                try
                {
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            idUsuario = Convert.ToInt32(reader["IdUsuario"]),
                            usuario = reader["Usuario"].ToString(),
                            contrasenaHash = reader["ContrasenaHash"] as byte[],
                            apellidoPaterno = reader["ApellidoPaterno"].ToString(),
                            apellidoMaterno = reader["ApellidoMaterno"].ToString(),
                            nombre = reader["Nombre"].ToString(),
                            idPerfil = Convert.ToInt32(reader["IdPerfil"]),
                            idEliminado = Convert.ToInt32(reader["IdEliminado"])
                        };
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener usuario por email: " + ex.Message);
                }
            }
            return usuario;
        }

        public void ActualizarContrasena(int idUsuario, string nuevaContrasenaPlano, int? idUsuarioModificacion)
        {
            if (string.IsNullOrEmpty(nuevaContrasenaPlano) ||
                nuevaContrasenaPlano.Length < 8 ||
                !nuevaContrasenaPlano.Any(char.IsUpper) ||
                !nuevaContrasenaPlano.Any(char.IsDigit) ||
                !nuevaContrasenaPlano.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                throw new Exception("La contraseña debe tener al menos 8 caracteres, incluir una mayúscula, un número y un carácter especial.");
            }

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ActualizarContrasena", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@NuevaContrasena", nuevaContrasenaPlano);
                cmd.Parameters.AddWithValue("@IdUsuarioModificacion", idUsuarioModificacion ?? (object)DBNull.Value);

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al actualizar la contraseña: " + ex.Message);
                }
            }
        }

        public Usuario ObtenerUsuarioPorId(int idUsuario)
        {
            Usuario usuario = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerUsuarioPorId", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

                try
                {
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            idUsuario = Convert.ToInt32(reader["IdUsuario"]),
                            usuario = reader["Usuario"].ToString(),
                            contrasenaHash = reader["ContrasenaHash"] as byte[],
                            apellidoPaterno = reader["ApellidoPaterno"].ToString(),
                            apellidoMaterno = reader["ApellidoMaterno"].ToString(),
                            nombre = reader["Nombre"].ToString(),
                            idPerfil = Convert.ToInt32(reader["IdPerfil"]),
                            idEliminado = Convert.ToInt32(reader["IdEliminado"])
                        };
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener usuario por ID: " + ex.Message);
                }
            }
            return usuario;
        }
    }
}


