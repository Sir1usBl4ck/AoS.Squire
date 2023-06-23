
using System.ComponentModel;
using AoS.Squire.Model;
using AoS.Squire.Services;
using AoS.Squire.Store;
using AoS.Squire.View;
using CommunityToolkit.Mvvm.Input;

namespace AoS.Squire.ViewModel;

public partial class GameSetupViewModel : BaseViewModel
{
    private readonly GameStore _gameStore;
    private readonly GameService _gameService;

    public GameSetupViewModel(GameStore gameStore,GameService gameService)
    {
        _gameStore = gameStore;
        _gameService = gameService;
        _gameStore.PropertyChanged += UpdateValues;
        
    }

    private void UpdateValues(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
        OnPropertyChanged(nameof(Player1Name));
        OnPropertyChanged(nameof(Player2Name));
        OnPropertyChanged(nameof(MissionName));
        StartGameCommand.NotifyCanExecuteChanged();
    }

    public string Player1Name => _gameStore.Player.Name;
    public string Player2Name => _gameStore.Opponent.Name;
    public string MissionName => !string.IsNullOrWhiteSpace(_gameStore?.SelectedMission?.Name) ? _gameStore?.SelectedMission?.Name: "Choose Mission" ;
    public string Player1Faction => !string.IsNullOrWhiteSpace(_gameStore.Player.Faction?.Name) ? _gameStore.Player.Faction?.Name: "Select Faction" ;
    public string Player2Faction => !string.IsNullOrWhiteSpace(_gameStore.Opponent.Faction?.Name) ? _gameStore.Opponent.Faction?.Name: "Select Faction" ;

    private bool CanStartGame()
    {
        return (_gameStore?.SelectedMission!=null && _gameStore?.Player?.Faction!=null && _gameStore?.Opponent?.Faction!=null);
    }

    [RelayCommand(CanExecute = nameof(CanStartGame))]
    private async Task StartGameAsync()
    {
        IsBusy = true;
        await _gameService.StartGame();
        await Shell.Current.GoToAsync(nameof(GamePage));
        IsBusy = false;

    }

    [RelayCommand]
    async Task GoToPlayerAllianceSelectionAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(AllianceSelectionPage)}", true, new Dictionary<string, object>
        {
            {"Player",_gameStore.Player}
        });
      
    }
    [RelayCommand]
    async Task GoToOpponentAllianceSelectionAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(AllianceSelectionPage)}", true, new Dictionary<string, object>
        {
            {"Player",_gameStore.Opponent}
        });
    }

    [RelayCommand]
    async Task GoToMissionPickerAsync()
    {
        await Shell.Current.GoToAsync(nameof(MissionPickerPage));
    }
}