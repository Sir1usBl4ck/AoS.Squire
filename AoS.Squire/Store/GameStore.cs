using AoS.Squire.Model;

namespace AoS.Squire.Store;

public class GameStore
{
    public List<Faction> Factions { get; set; }
    public int Round { get; set; }
    public Player TurnPlayer { get; set; }

}