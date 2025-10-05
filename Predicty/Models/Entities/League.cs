namespace Predicty.Models.Entities
{
    public class League
    {
        public int LeagueId { get; set; }
        public int ExternalLeagueID { get; set; }
        public int Season { get; set; }
        public string LeagueName { get; set; }
        public int OwnerId { get; set; }
        public DateTime CreatedDate { get; set; }

        //Owner: User
        public User Owner { get; set; }

        //Members: List<LeagueMember>
        public List<LeagueMember> LeagueMembers {  get; set; } 

        //Predictions: List<Prediction>
    }
}
