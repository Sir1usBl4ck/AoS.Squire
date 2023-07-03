using AoS.Squire.Model;
using AoS.Squire.Store;

namespace AoS.Squire.ViewModel;

public class VictoryPointViewModel : BaseViewModel
{
    private readonly VictoryPoint _victoryPoint;
    private readonly GameStore _store;  

    public VictoryPointViewModel(VictoryPoint victoryPoint, GameStore store)
    {
        _victoryPoint = victoryPoint;
        _store = store;
    }

    public string Description => _victoryPoint.Description;
    public int Points => _victoryPoint.Points;
    public bool IsScored
    {
        get => _victoryPoint.IsScored;
        set => _victoryPoint.IsScored = value;
    }
}