using Predicty.Repositories;
using System.Data;
using Predicty.Models.Dtos;
using Predicty.Models.Entities;
namespace Predicty.Services
{
    public class MatchService
    {
        private readonly FootballApiService _footballApi;
        private readonly MatchRepository _matchRepository;



        public MatchService(FootballApiService footballApi, MatchRepository matchRepository)
        {
            _footballApi = footballApi;
            _matchRepository = matchRepository;
        }

        public async Task<int> SyncMatchesFromApiAsync(int leagueId, int season)
        {
            //1. Get matches from API
            List<Match> allMatches = await _footballApi.GetMatchesByLeagueAndSeasonAsync(leagueId, season);

            //2. Filter only group stage matches and above
            var validRounds = new[]
           {
                "Group Stage",
                "Round of 16",
                "Quarter-finals",
                "Semi-finals",
                "Final"
            };

            //var filteredMatches = allMatches
            //.Where(m => validRounds.Contains(m.Round))
            //.ToList();

            var filteredMatches = allMatches
                .Where(m =>
                    !string.IsNullOrWhiteSpace(m.Round) && // avoid null/empty values
                    (m.Round.Trim().StartsWith("Group", StringComparison.OrdinalIgnoreCase) // any group stage
                     || validRounds.Contains(m.Round.Trim(), StringComparer.OrdinalIgnoreCase))) // knockout rounds
                .ToList();

            //3. Transform all the matches into a DataTable object
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ApiMatchId", typeof(int));
            dataTable.Columns.Add("ApiLeagueId", typeof(int));
            dataTable.Columns.Add("Season", typeof(int));
            dataTable.Columns.Add("Round", typeof(string));
            dataTable.Columns.Add("MatchDate", typeof(DateTime));
            dataTable.Columns.Add("HomeTeamId", typeof(int));
            dataTable.Columns.Add("AwayTeamId", typeof(int));
            dataTable.Columns.Add("HomeScore", typeof(int));
            dataTable.Columns.Add("AwayScore", typeof(int));

            foreach (var match in filteredMatches)
            {
                dataTable.Rows.Add(
                match.ExternalMatchID,
                match.ExternalLeagueID,
                match.Season,
                match.Round,
                match.MatchDateTime,
                match.HomeTeamID,
                match.AwayTeamID,
                (object?)match.HomeScore ?? DBNull.Value,
                (object?)match.AwayScore ?? DBNull.Value
            );
            }
            return await _matchRepository.AddMatchesAsync(dataTable);
        }
    }
}
