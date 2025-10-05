namespace Predicty.Models.Entities
{
    public class LeagueMember
    {
        public int LeagueId { get; set; }
        public int UserId { get; set; }
        public DateTime JoinedDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public League League { get; set; }
        public User User { get; set; }
    }
}
