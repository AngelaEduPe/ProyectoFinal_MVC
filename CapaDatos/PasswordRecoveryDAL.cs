using System;
using System.Data;
using System.Data.SqlClient;
using CapaEntidades;
using System.Configuration;

namespace CapaDatos
{
    public class PasswordRecoveryDAL
    {
        private readonly string cadena = ConfigurationManager.ConnectionStrings["cn"].ConnectionString;

        public void GuardarTokenRecuperacion(int idUsuario, string token, DateTime fechaExpiracion)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_GuardarTokenRecuperacion", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                cmd.Parameters.AddWithValue("@Token", token);
                cmd.Parameters.AddWithValue("@FechaExpiracion", fechaExpiracion);

                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al guardar el token de recuperación: " + ex.Message);
                }
            }
        }

        public PasswordRecoveryToken ObtenerTokenValido(string token)
        {
            PasswordRecoveryToken tokenRecuperado = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerTokenValido", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Token", token);

                try
                {
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        tokenRecuperado = new PasswordRecoveryToken
                        {
                            IdToken = Convert.ToInt32(reader["IdToken"]),
                            Token = reader["Token"].ToString(),
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                            FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
                            FechaExpiracion = Convert.ToDateTime(reader["FechaExpiracion"]),
                            Usado = Convert.ToBoolean(reader["Usado"])
                        };
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener el token de recuperación: " + ex.Message);
                }
            }
            return tokenRecuperado;
        }

        public void InvalidarToken(string token)
        {
            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_InvalidarToken", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Token", token);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al invalidar el token de recuperación: " + ex.Message);
                }
            }
        }
    }
}