using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace team_calendar.Models
{
    public class User : IdentityUser
    {
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        public string Address { get; set; }
        public long? FacebookId { get; set; }
    }
}