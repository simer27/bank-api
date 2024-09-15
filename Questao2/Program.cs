using Newtonsoft.Json;

public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014

        Console.ReadKey();

    }

    public static int getTotalScoredGoals(string team, int year)
    {
        var apiService = new FootballApiService();
        var goalCalculator = new GoalCalculator(apiService);

        return goalCalculator.GetTotalScoredGoals(team, year).Result;
    }

}