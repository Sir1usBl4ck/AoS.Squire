namespace AoS.Squire.Model;

public class FactionStats
{
    public string FactionName { get; set; }
    public int NumberOfGames { get; set; }
    public int Wins { get; set; }
    public int Draws { get; set; }
    public int Losses { get; set; }
    public decimal WinRate { get; set; }
    public string MostWinsAgainstFactionName { get; set; }
    public string MostLossesAgainstFactionName { get; set; }

}
public class BattleplanStats
{
    public string GhbName { get; set; }
    public string BattleplanName { get; set; }
    public int Wins { get; set; }
    public int Draw { get; set; }
    public int Losses { get; set; }
    public decimal WinRate { get; set; }
    public decimal AverageCompletedTactics { get; set; }
    public int TotalNumberOfGames { get; set; }
}

public class GlobalStats
{
    public int TotalNumberOfGames { get; set; }
    public int Wins { get; set; }
    public int Draws { get; set; }
    public int Losses { get; set; }
    public decimal WinRate { get; set; }
    public string MostWinsAgainstFactionName { get; set; }
    public string MostLossesAgainstFactionName { get; set; }
}