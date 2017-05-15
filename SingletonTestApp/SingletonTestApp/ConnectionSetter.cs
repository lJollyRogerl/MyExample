using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SingletonTestApp
{
    public class ConnectionSetter
    {
        public string ConnectionString { get; private set; }
        public static SqlConnection Connection { get; private set; }
        private static object locker = new object();
        public ConnectionSetter(string connection)
        {
            ConnectionString = connection;
        }

        public static SqlConnection GetConnection(string connectionString)
        {
            if (Connection == null)
            {
                lock (locker)
                {
                    if (Connection == null)
                    {
                        Connection = new SqlConnection(connectionString);
                    }
                }
            }
            return Connection;
        }
    }
}
