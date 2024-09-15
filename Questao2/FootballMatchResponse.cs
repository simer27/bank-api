using System.Collections.Generic;

public class FootballMatchResponse
{
	public List<FootballMatch> Data {  get; set; } = new List<FootballMatch>();
	public int TotalPages {  get; set; }
}
