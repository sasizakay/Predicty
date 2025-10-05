using Predicty.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Data;
using Microsoft.Data.SqlClient;
using Predicty.Models.Dtos;

namespace Predicty.Repositories
{
    public class UserRepository
    {
        private readonly DBServices _dbServices;
        SqlConnection con = new SqlConnection();
        public UserRepository(DBServices dbServices)
        {
            _dbServices = dbServices;
        }
        public async Task<User> AddUserAsync(User newUser)
        {
            SqlCommand cmd = new SqlCommand();
            con = _dbServices.SetConnection(con);

            cmd = CreateUserInsertCommandWithStoredProcedure("sp_User_insertUser", con, newUser);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return newUser;
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

        public async Task<UserDTO> GetUserByIDAsync(int userID)
        {
            con = _dbServices.SetConnection(con);
            SqlCommand cmd = new SqlCommand();

            cmd = CreateCommandWithStoredProcedure("sp_Users_GetUserByID", con);

            cmd.Parameters.AddWithValue("@userID", userID);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // execute the command
                dataReader.Read();
                UserDTO fetchedUser = new UserDTO
                {
                    UserId = userID,
                    UserName = dataReader["UserName"].ToString(),
                    Email = dataReader["Email"].ToString(),
                    CreatedDate = dataReader.GetDateTime(dataReader.GetOrdinal("CreatedDate"))
                };
                return fetchedUser;
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

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            con = _dbServices.SetConnection(con);
            SqlCommand cmd = new SqlCommand();
            List<UserDTO> users = new List<UserDTO>();

            //cmd = CreateCommandWithStoredProcedure("sp_Users_GetAllUsers", con);
            cmd = _dbServices.CreateCommandWithStoredProcedureWithoutParameters("sp_Users_GetAllUsers", con);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // execute the command
                while (dataReader.Read())
                {
                    UserDTO fetchedUser = new UserDTO
                    {
                        UserId = (int)dataReader["UserID"],
                        UserName = dataReader["UserName"].ToString(),
                        Email = dataReader["Email"].ToString(),
                        CreatedDate = dataReader.GetDateTime(dataReader.GetOrdinal("CreatedDate"))
                    };
                    users.Add(fetchedUser);
                }
                return users;
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

        private SqlCommand CreateUserInsertCommandWithStoredProcedure(String spName, SqlConnection con, User user)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@UserName", user.UserName);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@CreatedDate", user.CreatedDate);

            return cmd;
        }

        private SqlCommand CreateCommandWithStoredProcedure(String spName, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand(); // create the command object
            cmd.Connection = con;              // assign the connection to the command object
            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete
            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds
            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text
            return cmd;
        }
    }
}
