using System;
using System.Configuration;
using System.Linq;
using System.Data.SqlClient;
using System.Web;


namespace CapaDatos
{
    public class ConexionBD
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

        public SqlConnection Conectar()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);
        }
    }

}

