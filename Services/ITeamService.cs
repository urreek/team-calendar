using System.Collections.Generic;
using System.Threading.Tasks;
using team_calendar.Models;

namespace team_calendar.Services
{
    public interface ITeamService
    {
        Task<IEnumerable<Team>> GetAll();
    }
}