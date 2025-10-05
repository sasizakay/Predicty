using Predicty.Models.Entities;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using static Predicty.Controllers.LeaguesController;
using System.Reflection.PortableExecutable;
namespace Predicty.Repositories

{
    public class TeamRepository
    {
        private readonly DBServices _dbServices;
        SqlConnection con = new SqlConnection();
        public TeamRepository(DBServices dbServices)
        {
            _dbServices = dbServices;
        }

        //public async Task<List<Team>> GetAllTeamsAsync()
        //{
        //    //return await _dbContext.Teams.ToListAsync();
        //}

        //public async Task<Team?> GetTeamByIdAsync(int id)
        //{
        //    //return await _dbContext.Teams.FindAsync(id);
        //}

        public async Task<int> AddTeamsAsync(DataTable dt)
        {
            con = _dbServices.SetConnection(con);
            SqlCommand cmd = new SqlCommand();

            cmd = CreateTeamsBatchInsertCommandWithStoredProcedure("sp_Teams_InsertBatch", con, dt);             // create the command

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

        public SqlCommand CreateTeamsBatchInsertCommandWithStoredProcedure(String spName, SqlConnection con, DataTable dt)
        {
            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            // TVP parameter
            var tvpParam = new SqlParameter
            {
                ParameterName = "@Teams",           // must match SP parameter name
                SqlDbType = SqlDbType.Structured,   // important
                TypeName = "dbo.TeamTableType",     // must match your TVP name
                Value = dt
            };

            cmd.Parameters.Add(tvpParam);


            return cmd;

        }

    }
}
