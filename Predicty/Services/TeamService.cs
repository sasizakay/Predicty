using Predicty.Models.Entities;
using Predicty.Repositories;
using System.Data;

namespace Predicty.Services
{
    public class TeamService
    {
        private readonly FootballApiService _footballApi;
        private readonly TeamRepository _teamRepository;

        

        public TeamService(FootballApiService footballApi, TeamRepository teamRepository)
        {
            _footballApi = footballApi;
            _teamRepository = teamRepository;
        }

        public async Task<int> SyncTeamsFromApiAsync(int leagueId, int season)
        {
            // 1. Get teams from API
            var apiTeams = await _footballApi.GetTeamsByLeagueAndSeasonAsync(leagueId, season);
            //2. Transform all the teams into a DataTable object
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ApiTeamId", typeof(int));
            dataTable.Columns.Add("TeamName", typeof(string));
            dataTable.Columns.Add("LogoUrl", typeof(string));

            foreach (var team in apiTeams)
            {
                dataTable.Rows.Add(team.TeamID, team.TeamName, team.LogoURL ?? (object)DBNull.Value);
            }

            return await _teamRepository.AddTeamsAsync(dataTable);
        }
    }
}
