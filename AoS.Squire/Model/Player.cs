using CommunityToolkit.Mvvm.ComponentModel;

namespace AoS.Squire.Model;

public partial class Player : ObservableObject
{
    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private Faction _faction;

    [ObservableProperty] 
    private bool _isGranStrategyCompleted;

    public bool IsOpponent { get; set; }

    public List<BattleTactic> AvailableBattleTactics { get; set; } = new();

    public int Id { get; set; }
    
}

