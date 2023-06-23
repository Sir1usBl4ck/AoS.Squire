using CommunityToolkit.Mvvm.ComponentModel;

namespace AoS.Squire.Model;

public partial class BattleTactic : ObservableObject
{
    [ObservableProperty]
    private bool _isAvailable ;
    public string Name { get; set; }
    public string Description { get; set; }
}