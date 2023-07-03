using AoS.Squire.Services;
using AoS.Squire.Store;
using AoS.Squire.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AoS.Squire.ViewModel;

[QueryProperty("Turn","Turn")]
public partial class TurnPageViewModel : BaseViewModel
{
    private readonly GameStore _gameStore;
    private readonly GameService _gameService;

    public TurnPageViewModel(GameStore gameStore, GameService gameService)
    {
        _gameStore = gameStore;
        _gameService = gameService;
    }
    [RelayCommand]
    private async Task GoToTacticsPicker(TurnViewModel turn)
    {
        await Shell.Current.GoToAsync($"{nameof(TacticsPickerPage)}", true, new Dictionary<string, object>
        {
            {"Turn",turn}
        });
    }
    
    [ObservableProperty]
    private TurnViewModel _turn;

    public void CalculateScore()
    {
        _gameStore.CalculateGameScore();
    }
}