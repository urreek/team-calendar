using System.ComponentModel.DataAnnotations;

namespace team_calendar.Dtos
{
    public class FacebookDto
    {   
        [Required]
        public string AccessToken { get; set; }
    }
}