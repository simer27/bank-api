using System.Threading.Tasks;

public interface IGoalCalculator 
{
	Task<int> GetTotalScoredGoals(string team, int year);
}
