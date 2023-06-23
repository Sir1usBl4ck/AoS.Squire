namespace AoS.Squire.Model;

public class BattleRound
{
    public int RoundNumber { get; set; }
    public Turn PlayerTurn { get; set; }
    public Turn OpponentTurn { get; set; }
}