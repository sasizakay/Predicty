namespace Predicty.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<League> LeaguesOwned { get; set; } // Leagues this user owns
        
        public List<LeagueMember> LeagueMembership { get; set; } //: Leagues this user is a member in
        //public List<Prediction> Predictions
    }
}
