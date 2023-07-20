namespace AoS.Squire.Model;

public class Game
{
    public Game(Player player, Player opponent, Battleplan battleplan)
    {
        Player = player;
        Opponent = opponent;
        Battleplan = battleplan;
    }
    public Player Player { get; }
    public Player Opponent { get; }
    public List<BattleRound> BattleRounds { get; } = new();
    public Battleplan Battleplan { get; set; }

    public int PlayerScore { get; set; }
    public int OpponentScore { get; set; }
    public void CalculateScore()
    {
        PlayerScore = BattleRounds.Select(r => r.PlayerTurn).Select(p => p.Score).Sum() + (Player.IsGranStrategyCompleted ? 3 : 0);

        OpponentScore = BattleRounds.Select(r => r.OpponentTurn).Select(p => p.Score).Sum() + (Opponent.IsGranStrategyCompleted ? 3 : 0);

    }
}