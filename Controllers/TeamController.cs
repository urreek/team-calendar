
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using team_calendar.Models;
using team_calendar.Services;

namespace team_calendar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : Controller
    {
        private ITeamService teamService;
        public TeamController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        [HttpGet]
        public async Task<IEnumerable<Team>> GetTeams() => await teamService.GetAll();

    }

}