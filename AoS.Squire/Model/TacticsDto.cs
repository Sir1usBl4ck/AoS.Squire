namespace AoS.Squire.Model;

public class TacticsDto
{
    public int FactionId { get; set; }
    public string FactionName { get; set; }
    public string AllianceName { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool HasExtraTracker { get; set; }
    public string ExtraTrackerName { get; set; }
}