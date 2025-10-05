using Predicty.Models.Entities;
using Predicty.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Data;
using Microsoft.Data.SqlClient;
using static Predicty.Controllers.LeaguesController;

namespace Predicty.Repositories
{
    public class LeagueRepository
    {
        private readonly DBServices _dbServices;
        SqlConnection con = new SqlConnection();

        public LeagueRepository(DBServices dbServices)
        {
            _dbServices = dbServices;
        }

        
        public async Task<League> AddLeagueAsync(League newLeague)
        {
            //connection();
            con = _dbServices.SetConnection(con);
            SqlCommand cmd = new SqlCommand();

            cmd = CreateLeagueInsertCommandWithStoredProcedure("sp_Leagues_InsertLeague", con, newLeague);             // create the command

            var leagueIdParam = new SqlParameter("@LeagueID", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            cmd.Parameters.Add(leagueIdParam);

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                newLeague.LeagueId = (int)leagueIdParam.Value;
                return newLeague;
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
        public async Task<LeagueMember> AddMemberToLeagueAsync(AddMemberToLeagueRequest request)
        {
            //_dbServices.SetConnectionParameters();
            con = _dbServices.SetConnection(con);
            SqlCommand cmd = new SqlCommand();

            cmd = CreateAddMemberToLeagueCommandWithStoredProcedure("sp_LeagueMembers_InsertLeagueMember", con, request);             // create the command



            LeagueMember AddedLM = new LeagueMember
            {
                LeagueId = request.LeagueID,
                UserId = request.UserID
            };
            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command

                return AddedLM;
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

        public async Task<List<LeagueDTO>> GetLeaguesByUserAsync(int userID)
        {
            con = _dbServices.SetConnection(con);
            SqlCommand cmd = new SqlCommand();
            List<LeagueDTO> leagues = new List<LeagueDTO>();

            cmd = _dbServices.CreateCommandWithStoredProcedureWithoutParameters("sp_Leagues_GetLeaguesByUser", con);

            cmd.Parameters.AddWithValue("@UserID", userID);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // execute the command
                while (dataReader.Read())
                {
                    LeagueDTO fetchedLeagues = new LeagueDTO
                    {
                        LeagueId = (int)dataReader["LeagueID"],
                        ExternalLeagueID = (int)dataReader["ExternalLeagueID"],
                        Season = (int)dataReader["Season"],
                        LeagueName = dataReader["LeagueName"].ToString(),
                        OwnerId = (int)dataReader["OwnerId"],
                        CreatedDate = dataReader.GetDateTime(dataReader.GetOrdinal("CreatedDate"))
                    };
                    leagues.Add(fetchedLeagues);
                }
                return leagues;
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



        public SqlCommand CreateLeagueInsertCommandWithStoredProcedure(String spName, SqlConnection con, League league)
        {
            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@ExternalLeagueID", league.ExternalLeagueID);
            cmd.Parameters.AddWithValue("@Season", league.Season);
            cmd.Parameters.AddWithValue("@LeagueName", league.LeagueName);
            cmd.Parameters.AddWithValue("@OwnerID", league.OwnerId);

            return cmd;
        }

        public SqlCommand CreateAddMemberToLeagueCommandWithStoredProcedure(String spName, SqlConnection con, AddMemberToLeagueRequest request)
        {
            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@LeagueID", request.LeagueID);
            cmd.Parameters.AddWithValue("@UserID", request.UserID);
            
            

            return cmd;
        }

        private void connection()
        {
            con = _dbServices.SetConnection(con);
        }

    }
}
