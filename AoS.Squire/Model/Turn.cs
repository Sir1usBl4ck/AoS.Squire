namespace AoS.Squire.Model;

public class Turn
{
    public Turn(int id, BattleRound battleRound, Player player)
    {
        Id = id;
        BattleRound = battleRound;
        Player = player;
    }

    public int Id { get; set; }
    public BattleRound BattleRound { get; }
    public int RoundNumber => BattleRound.RoundNumber;
    public Player Player { get; }
    public BattleTactic SelectedBattleTactic { get; set; }
    public int Score => CalculateScore();
    public List<VictoryPoint> VictoryPoints { get; set; } = new();
    public bool GoingFirst { get; set; }
    public int ExtraPoints { get; set; }
    public bool HasExtraPoints { get; set; }

    private int CalculateScore()
    {
        var result = VictoryPoints.Where(v => v.IsScored).Select(v => v.Points).Sum() + (HasExtraPoints ? ExtraPoints : 0);

        return result;

    }
}