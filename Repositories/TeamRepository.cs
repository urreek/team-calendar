using Microsoft.EntityFrameworkCore;
using team_calendar.Models;

namespace team_calendar.Repositories
{
    public class TeamRepository : Repository<Team>, ITeamRepository
    {
        public TeamRepository(DbContext context) : base(context)
        {
        }
    }
}