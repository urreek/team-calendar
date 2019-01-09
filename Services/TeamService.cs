using System.Collections.Generic;
using team_calendar.Models;
using team_calendar.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace team_calendar.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            this.teamRepository = teamRepository;
        }

        public Team Get(int id) => teamRepository.Get(id);
        public async Task<IEnumerable<Team>> GetAll() => await teamRepository.GetAll();

        public void Add(Team team) => teamRepository.Add(team);

        public void Remove(Team team) => teamRepository.Remove(team);
    }
}