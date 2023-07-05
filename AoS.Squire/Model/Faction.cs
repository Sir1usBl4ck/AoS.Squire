namespace AoS.Squire.Model;

public class Faction
{
    public int Id { get; set; }
    public string AllianceName { get; set; }
    public string Name { get; set; }
    public List<BattleTactic> BattleTactics { get; set; }
    public bool HasExtraTracker { get; set; }
    public string ExtraTrackerName { get; set; }
    public bool IsFavorite { get; set; }
}