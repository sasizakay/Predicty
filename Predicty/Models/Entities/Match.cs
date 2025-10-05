using System;

namespace Predicty.Models.Entities
{
    public class Match
    {
        public int MatchId { get; set; } // מזהה פנימי אצלך (PK)
        public int ExternalMatchID { get; set; } // מזהה מה-API
        public int ExternalLeagueID { get; set; } // מזהה מה-API
        public int Season { get; set; }
        public string Round { get; set; }
        public DateTime MatchDateTime { get; set; }
        public string HomeTeamID { get; set; }
        public string AwayTeamID { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public string Status { get; set; } // למשל NS / FT / LIVE

        //HomeTeam: Team
        Team HomeTeam { get; set; }

        //AwayTeam: Team
        Team AwayTeam { get; set; }

        //Predictions: List<Prediction>
        List<Prediction> Predictions { get; set; }
    }
}
