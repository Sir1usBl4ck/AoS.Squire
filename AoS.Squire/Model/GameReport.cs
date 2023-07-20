using SQLite;

namespace AoS.Squire.Model;

public class BattleRoundReport
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [Indexed]
    public int GameReportId { get; set; }
    public int RoundNumber { get; set; }
    public int PlayerScore { get; set; }
    public int OpponentScore { get; set; }
    public bool PlayerPriority { get; set; }
    public bool OpponentPriority { get; set; }
}

public class GameReport
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int PlayerFactionId { get; set; }
    public int OpponentFactionId { get; set; }
    public int PlayerScore { get; set; }
    public int OpponentScore { get; set; }
    public string Battleplan { get; set; }
    public int PlayerBattleTacticsCount { get; set; }
    public int OpponentBattleTacticsCount { get; set; }
}