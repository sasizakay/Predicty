using Microsoft.AspNetCore.Mvc;
using Predicty.Services;
using static Predicty.Controllers.TeamsController;

namespace Predicty.Controllers
{
    [Route("matches")]
    public class MatchesController : Controller
    {
        private readonly MatchService _matchService;

        public MatchesController(MatchService matchService)
        {
            _matchService = matchService;
        }

        /// <summary>
        /// Fetches Matches by league and season from the external API and saves them in the DB
        /// </summary>
        [HttpPost("add-next-matches")]
        public async Task<IActionResult> SyncMatches([FromBody] SyncRequest request)
        {
            int matchesAdded = await _matchService.SyncMatchesFromApiAsync(request.LeagueId, request.Season);
            return Ok(new { Message = $"Matches synced successfully, {matchesAdded} new matches added to the database!" });
        }

    }
}
