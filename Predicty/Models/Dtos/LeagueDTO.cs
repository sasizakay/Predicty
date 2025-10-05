namespace Predicty.Models.Dtos
{
    public class LeagueDTO
    {
        public int LeagueId { get; set; }
        public int ExternalLeagueID { get; set; }
        public int Season { get; set; }
        public string LeagueName { get; set; }
        public int OwnerId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
