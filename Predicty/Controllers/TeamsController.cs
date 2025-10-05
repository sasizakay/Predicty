using Predicty.Services;
using Microsoft.AspNetCore.Mvc;

namespace Predicty.Controllers
{
    [Route("teams")]
    public class TeamsController : Controller
    {
        private readonly TeamService _teamService;

        private readonly FootballApiService _apiService;
        public TeamsController(TeamService teamService, FootballApiService apiService)
        {
            _teamService = teamService;
            _apiService = apiService;
        }

        /// <summary>
        /// Fetches Teams by league and season from the external API and saves them in the DB
        /// </summary>
        [HttpPost("sync-teams")]
        public async Task<IActionResult> SyncTeams([FromBody] SyncRequest request)
        {
            int teamsAdded = await _teamService.SyncTeamsFromApiAsync(request.LeagueId, request.Season);
            return Ok(new { Message = $"Teams synced successfully, {teamsAdded} new teams added to the database!" });
        }


        public class SyncRequest
        {
            public int LeagueId { get; set; }
            public int Season { get; set; }
        }

    }
}
