using System;
using System.Threading.Tasks;

public class GoalCalculator : IGoalCalculator
{
    private readonly IFootballApiService _apiService;

    public GoalCalculator(IFootballApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<int> GetTotalScoredGoals(string team, int year)
    {
        int totalGoals = 0;
        int currentPage = 1;
        int totalPages;

        do
        {
            FootballMatchResponse matchResponse = await _apiService.GetMatchesAsync(team, year, currentPage);

            foreach (var match in matchResponse.Data)
            {
                if (int.TryParse(match.Team1Goals, out int goals))
                {
                    totalGoals += goals;
                }
            }


            totalPages = matchResponse.TotalPages;
            currentPage++;
        }while (currentPage <= totalGoals);

       return totalGoals;
    }
}
