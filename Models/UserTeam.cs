namespace team_calendar.Models
{
    public class UserTeam {
        public string UserId { get; set; }
        public User User { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}