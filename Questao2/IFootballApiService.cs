using System.Threading.Tasks;

public interface IFootballApiService
{
	Task<FootballMatchResponse> GetMatchesAsync(string team, int year, int page);
}