using System;

namespace Predicty.Models.Entities
{
    public class Team
    {
        public int TeamID { get; set; }             // מזהה הקבוצה (מה-API)
        public string TeamName { get; set; } = string.Empty; // שם הקבוצה
        public string? LogoURL { get; set; } = string.Empty; // קישור ללוגו

        // קשרים
        public ICollection<Match> HomeMatches { get; set; } = new List<Match>();
        public ICollection<Match> AwayMatches { get; set; } = new List<Match>();
    }
}
