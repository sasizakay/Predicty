using Predicty.Models.Dtos;
using Predicty.Services;
using Microsoft.AspNetCore.Mvc;

namespace Predicty.Controllers
{
    [Route("leagues")]
    public class LeaguesController : Controller
    {
        private readonly LeagueService _leagueService;

        public LeaguesController(LeagueService leagueService)
        {
            _leagueService = leagueService;
        }

        #region Get methods

        #endregion


        #region Post methods

        /// <summary>
        /// Creates a new league, given league name and owner id (user)
        /// </summary>
        [HttpPost("create-league")]
        public async Task<IActionResult> CreateLeague([FromBody] LeagueCreationRequest request)
        {
            //LeagueDTO league = await _leagueService.CreateLeagueAsync(request.LeagueName, request.OwnerId);
            LeagueDTO league = await _leagueService.CreateLeagueAsync(request);
            return Ok(league);
        }

        /// <summary>
        /// Adds a user to a league, making it a league member
        /// </summary>
        [HttpPost("add-member-to-league")]
        public async Task<IActionResult> AddMemberToLeague([FromBody] AddMemberToLeagueRequest request)
        {
            //LeagueDTO league = await _leagueService.CreateLeagueAsync(request.LeagueName, request.OwnerId);
            LeagueMemberDTO lmDTO = await _leagueService.AddMemberToLeagueAsync(request);
            return Ok(lmDTO);
        }

        [HttpPost("get-leagues-by-user")]
        public async Task<IActionResult> GetLeaguesByUser(int userID)
        {
            List<LeagueDTO> leagueDTOs = await _leagueService.GetLeaguesByUserAsync(userID);
            return Ok(leagueDTOs);
        }

        #endregion

        //Request objects
        public class LeagueCreationRequest
        {
            public string LeagueName { get; set; }
            public int OwnerId { get; set; }
            public int ExternalLeagueID { get; set; }
            public int Season { get; set; }
        }

        public class AddMemberToLeagueRequest
        {
            public int UserID { get; set; }
            public int LeagueID { get; set; }
        }
    }
}
