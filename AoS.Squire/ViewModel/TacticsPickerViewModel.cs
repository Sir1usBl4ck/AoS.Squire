using System.Collections.ObjectModel;
using AoS.Squire.Model;
using AoS.Squire.Services;
using AoS.Squire.Store;
using CommunityToolkit.Mvvm.Input;

namespace AoS.Squire.ViewModel;

[QueryProperty("Turn","Turn")]

public partial class TacticsPickerViewModel : BaseViewModel
{
    private readonly GameStore _gameStore;
    private readonly GameService _gameService;
    private TurnViewModel _turn;
        
    public TacticsPickerViewModel(GameStore gameStore, GameService gameService)
    {
        _gameStore = gameStore;
        _gameService = gameService;
    }
    public ObservableCollection<BattleTactic> AvailableBattleTactics { get; set; } = new();

    public TurnViewModel Turn
    {
        get => _turn;
        set
        {
            if (Equals(value, _turn)) return;
            _turn = value;
            OnPropertyChanged();
            PopulateBattleTactics(_turn);
        }
    }
    [RelayCommand]
    private async Task SelectionChangedAsync(BattleTactic tactic)
    {
        _gameService.SelectTactic(Turn.Turn,tactic);
        PopulateBattleTactics(Turn);
        Turn.PopulateBattleTacticInfo(tactic);
        
        
        await Shell.Current.GoToAsync("..");
    }

    private void PopulateBattleTactics(TurnViewModel turn)
    {
        AvailableBattleTactics.Clear();
        foreach (var tactic in turn.PlayerTactics.Where(t=>t.IsAvailable))
        {
            AvailableBattleTactics.Add(tactic);
        }
    }


}