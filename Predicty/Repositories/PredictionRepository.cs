using Predicty.Models.Entities;
using Microsoft.Data.SqlClient;


namespace Predicty.Repositories
{
    public class PredictionRepository
    {
        private readonly DBServices _dbServices;
        SqlConnection con = new SqlConnection();

        public PredictionRepository(DBServices dbServices)
        {
            _dbServices = dbServices;
        }
        //Future addition to handle a prediction from the user (insert new or update existing one)
        //public async Task<Prediction> AddPredictionAsync()
        //{
        //    con = _dbServices.SetConnection(con);
        //    SqlCommand cmd = new SqlCommand();
        //}
    }
}
