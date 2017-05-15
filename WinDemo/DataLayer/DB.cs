using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataLayer
{
    public class DB
    {
        public static string ConnectionString
        {
            get
            {
                string connStr = ConfigurationManager.ConnectionStrings["AWConnection"].ToString();
                SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder(connStr);
                sb.ApplicationName = ApplicationName ?? sb.ApplicationName;
                sb.ConnectTimeout = (ConnectionTimeout > 0) ? ConnectionTimeout : sb.ConnectTimeout;
                return sb.ToString();
            }
        }
        /// <summary>
        /// Returns opened SqlConnection
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetSqlConnection()
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString);
                conn.Open();
                return conn;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
           
        }

        /// <summary>
        /// Overrides ConnectTimeout of current connection
        /// </summary>
        public static int ConnectionTimeout { get; set; }

        /// <summary>
        /// Overrides current Application name
        /// </summary>
        public static string ApplicationName { get; set; }
    }
}
