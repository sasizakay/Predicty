using Predicty.Models.Dtos;
using Predicty.Models.Entities;
using Predicty.Repositories;
using System.Collections.Generic;
using static Predicty.Controllers.LeaguesController;

namespace Predicty.Services
{
    public class LeagueService
    {
        private readonly LeagueRepository _leagueRepository;
        public LeagueService(LeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }
        public async Task<LeagueDTO> CreateLeagueAsync(LeagueCreationRequest leagueRequest)
        {
            League newLeague = new League
            {
                LeagueName = leagueRequest.LeagueName,
                OwnerId = leagueRequest.OwnerId,
                ExternalLeagueID = leagueRequest.ExternalLeagueID,
                Season = leagueRequest.Season,
                CreatedDate = DateTime.UtcNow
            };
            League addedLeague = await _leagueRepository.AddLeagueAsync(newLeague);
            //Adding the user who opened the league to the league as a league member
            AddMemberToLeagueRequest lmRequest = new AddMemberToLeagueRequest
            {
                LeagueID = addedLeague.LeagueId,
                UserID = addedLeague.OwnerId
            };
            await _leagueRepository.AddMemberToLeagueAsync(lmRequest);

            //Transforming the data from League object (which was added previously to the DB)
            //To a League DTO object which will be returned to the API
            LeagueDTO returnLeagueDTO = new LeagueDTO
            {
                LeagueName = addedLeague.LeagueName,
                OwnerId = addedLeague.OwnerId,
                CreatedDate = addedLeague.CreatedDate
            };
            return returnLeagueDTO;
        }

        public async Task<LeagueMemberDTO> AddMemberToLeagueAsync(AddMemberToLeagueRequest request)
        {
            LeagueMember AddedLM = await _leagueRepository.AddMemberToLeagueAsync(request);
            LeagueMemberDTO returnedLmDTO = new LeagueMemberDTO
            {
                UserID = AddedLM.UserId,
                LeagueID = AddedLM.LeagueId
            };
            return returnedLmDTO;
        }

        public async Task<List<LeagueDTO>> GetLeaguesByUserAsync(int userID)
        {
            return await _leagueRepository.GetLeaguesByUserAsync(userID);
        }
    }
}
