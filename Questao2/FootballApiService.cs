using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

public class FootballApiService : IFootballApiService
    {
        private const string BaseUrl = "https://jsonmock.hackerrank.com/api/football_matches";

        public async Task<FootballMatchResponse> GetMatchesAsync(string team, int year, int page)
    {
            string url = $"{BaseUrl}?year={year}&team1={team}&page={page}";

            using (HttpClient client = new HttpClient()) 
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<FootballMatchResponse>(jsonResponse);
                }
                else
                {
                throw new HttpRequestException("Erro no acesso à Api!");
                }
            }
        }
    }
