using System.Data;
using Microsoft.Data.SqlClient;
namespace Predicty.Repositories
{
    public class MatchRepository
    {
        private readonly DBServices _dbServices;
        SqlConnection con = new SqlConnection();
        public MatchRepository(DBServices dbServices)
        {
            _dbServices = dbServices;
        }

        public async Task<int> AddMatchesAsync(DataTable dt)
        {
            con = _dbServices.SetConnection(con);
            SqlCommand cmd = new SqlCommand();

            cmd = CreateMatchesBatchInsertCommandWithStoredProcedure("sp_Matches_InsertBatch", con, dt);             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // execute the command
                int insertedCount = 0;

                if (dataReader.Read())
                {
                    insertedCount = dataReader.GetInt32(0); // first column of the first row
                }
                return insertedCount;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        public SqlCommand CreateMatchesBatchInsertCommandWithStoredProcedure(String spName, SqlConnection con, DataTable dt)
        {
            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            // TVP parameter
            var tvpParam = new SqlParameter
            {
                ParameterName = "@Matches",           // must match SP parameter name
                SqlDbType = SqlDbType.Structured,   // important
                TypeName = "dbo.MatchType",     // must match your TVP name
                Value = dt
            };

            cmd.Parameters.Add(tvpParam);


            return cmd;

        }
    }
}
