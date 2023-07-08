namespace AoS.Squire.Model;

public class GlobalStats
{
    public int TotalNumberOfGames { get; set; }
    public decimal Wins { get; set; }
    public int Draws { get; set; }
    public int Losses { get; set; }
    public decimal WinRate { get; set; }
    public string MostWinsAgainstFactionName { get; set; }
    public string MostLossesAgainstFactionName { get; set; }
}