namespace AoS.Squire.Model;

public  class Turn 
{
    public Turn(int id,BattleRound battleRound, Player player)
    {
        Id = id;
        BattleRound = battleRound;
        Player = player;
    }

    public int Id { get; set; }
    public BattleRound BattleRound { get; }
    public int RoundNumber => BattleRound.RoundNumber;
    public Player Player { get;}
    public BattleTactic SelectedBattleTactic { get; set; }
    public int Score => VictoryPoints.Where(v=>v.IsScored).Select(v=>v.Points).Sum();
    public List<VictoryPoint> VictoryPoints { get; set; } = new();
    public bool GoingFirst { get; set; }
}