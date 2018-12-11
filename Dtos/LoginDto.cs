using System.ComponentModel.DataAnnotations;

namespace team_calendar.Dtos
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}