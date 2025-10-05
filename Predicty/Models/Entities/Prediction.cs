namespace Predicty.Models.Entities
{
    public class Prediction
    {
        public int PredictionId { get; set; }
        public int UserId { get; set; }
        public int LeagueId { get; set; }
        public int MatchId { get; set; }
        public int PredictionHomeScore { get; set; }
        public int PredictionAwayScore { get; set; }
        public DateTime PredictionDate { get; set; }

        //User: User

        //League: League

        //Match: Match
    }
}
