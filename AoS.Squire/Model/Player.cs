namespace AoS.Squire.Model;

public class Game
{
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    public List<BattleRound> BattleRounds { get; set; }
}
public class Faction
{
    public string Name { get; set; }
    public List<BattleTactic> BattleTactics { get; set; }
}

public class BattleTactic
{
    public bool IsUsed { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class BattleRound
{
    public int RoundNumber { get; set; }
    public Player PriorityPlayer { get; set; }
    public Turn Turn1 { get; set; }
    public Turn Turn2 { get; set; }
}

public class Turn
{
    public List<ScoreGenerator> ScoreGenerators { get; set; }

}

public class ScoreGenerator
{
    public string Name { get; set; }
    public int Points { get; set; }
    public bool IsScored { get; set; }
}

public class Player
{
    public string Name { get; set; }
    public Faction Faction { get; set; }
}

