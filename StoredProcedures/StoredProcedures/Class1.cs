using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;

public class StoredProcedures
    {
        [SqlProcedure]
        public static int CountEmployess()
        {
            try
            {
                int rows = 0;
                SqlConnection connection = new SqlConnection("Context Connection=true");
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select count(*) as 'Количество сотрудников' "
                + "from Employee";
                rows = (int)cmd.ExecuteScalar();

                connection.Close();
                return rows;
            }
            catch 
            {
                return -1;
            }
        }
}

public class BudgetPercent
{
    private const int percent = 12;

    public static SqlDouble ComputeBudget(float budget)
    {
        return budget * percent;
    }
}