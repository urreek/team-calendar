
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace team_calendar.Models
{
    public class Team {

        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        public List<UserTeam> UserTeams { get; set; }
    }
}