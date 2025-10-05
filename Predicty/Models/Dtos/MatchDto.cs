namespace Predicty.Models.Dtos
{
    public class MatchDto
    {
        public int ApiMatchId { get; set; }
        public int ApiLeagueId { get; set; }
        public int Season { get; set; }
        public string Round { get; set; }
        public DateTime MatchDate { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
    }
}
