using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Predicty.Models; // לוודא שה־DTOs נמצאים בתיקיה זו
using Predicty.Models.Entities;
using Predicty.Models.Dtos;
using System.Text.Json;
using System.Data;

namespace Predicty.Services
{
    public class FootballApiService
    {
        private readonly HttpClient _httpClient;
        private readonly DBServices _dbServices;
        private const string ApiHost = "https://v3.football.api-sports.io";
        private const string ApiKey = "7a7e970b60c581f671f898df80e435f1"; // החלף במפתח שלך

        public FootballApiService(HttpClient httpClient, DBServices dbServices)
        {
            _httpClient = httpClient;
            _dbServices = dbServices;
            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-host", ApiHost);
            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", ApiKey);
        }

        public async Task<List<MatchDto>> GetClubWorldCupFixturesAsync()
        {
            var uri = $"{ApiHost}/fixtures?league=15&season=2025";
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var apiResult = JsonConvert.DeserializeObject<ApiResponseDto>(content);

            return apiResult?.Response ?? new List<MatchDto>();
        }

        public async Task<List<Team>> GetTeamsByLeagueAndSeasonAsync(int leagueId, int season)
        {
            var uri = $"{ApiHost}/teams?league={leagueId}&season={season}";
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            // Deserialize JSON response
            var jsonDoc = JsonDocument.Parse(content);
            var teamsList = new List<Team>();

            foreach (var teamElement in jsonDoc.RootElement.GetProperty("response").EnumerateArray())
            {
                var teamData = teamElement.GetProperty("team");
                var team = new Team
                {
                    TeamID = teamData.GetProperty("id").GetInt32(),
                    TeamName = teamData.GetProperty("name").GetString(),
                    LogoURL = teamData.GetProperty("logo").GetString()
                };
                teamsList.Add(team);
            }
            return teamsList;
        }

        public async Task<List<Match>> GetMatchesByLeagueAndSeasonAsync(int leagueId, int season)
        {
            var uri = $"{ApiHost}/fixtures?league={leagueId}&season={season}";
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            // Deserialize JSON response
            var jsonDoc = JsonDocument.Parse(content);
            var matchesList = new List<Match>();
            var responseArray = jsonDoc.RootElement.GetProperty("response").EnumerateArray();

            foreach (var matchElement in responseArray)
            {
                var fixture = matchElement.GetProperty("fixture");
                var league = matchElement.GetProperty("league");
                var teams = matchElement.GetProperty("teams");
                var match = new Match
                {
                    //TeamID = teamData.GetProperty("id").GetInt32(),
                    //TeamName = teamData.GetProperty("name").GetString(),
                    //LogoURL = teamData.GetProperty("logo").GetString()
                    ExternalMatchID = fixture.GetProperty("id").GetInt32(),
                    ExternalLeagueID = leagueId,
                    Season = season,
                    Round = league.GetProperty("round").GetString(),
                    MatchDateTime = fixture.GetProperty("date").GetDateTime(),
                    HomeTeamID = teams.GetProperty("home").GetProperty("id").GetInt32().ToString(),
                    AwayTeamID = teams.GetProperty("away").GetProperty("id").GetInt32().ToString(),
                    Status = fixture.GetProperty("status").GetProperty("short").GetString()
                };
                matchesList.Add(match);
            }
            return matchesList;
        }
    }
}
