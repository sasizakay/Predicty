
//using MEDIQUICK.BL;
//using MEDIQUICK.Controllers;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

public class DBServices
{
    public DBServices() { }
    public SqlConnection SetConnection(SqlConnection con)
    {
        try
        {
            con = connect(); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        return con;

    }
    public SqlConnection connect()
    {
        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("DefaultConnection");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    public SqlCommand CreateCommandWithStoredProcedureWithoutParameters(String spName, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        return cmd;
    }

    public void SetConnectionParameters()
    {
        SqlConnection con = new SqlConnection();
        con = SetConnection(con);
    }


    //public class SqlConnectionObject
    //{
    //    public SqlConnection Con {  get; set; }
    //    public SqlCommand Cmd { get; set; }
    //}

}



