
using System.ComponentModel;
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
        _gameStore.Player.PropertyChanged += UpdateValues;
        _gameStore.Opponent.PropertyChanged += UpdateValues;
        
    }

    private void UpdateValues(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
        OnPropertyChanged(nameof(IsPlayerSelected));
        OnPropertyChanged(nameof(IsOpponentSelected));
        OnPropertyChanged(nameof(MissionName));
        StartGameCommand.NotifyCanExecuteChanged();
    }

    public string MissionName => !string.IsNullOrWhiteSpace(_gameStore?.Battleplan?.Name) ? _gameStore?.Battleplan?.Name: "Choose Battleplan" ;
    public string PlayerFaction => !string.IsNullOrWhiteSpace(_gameStore.Player.Faction?.Name) ? _gameStore.Player.Faction?.Name: "Select Faction" ;
    public string OpponentFaction => !string.IsNullOrWhiteSpace(_gameStore.Opponent.Faction?.Name) ? _gameStore.Opponent.Faction?.Name: "Select Faction" ;

    public bool IsPlayerSelected => !string.IsNullOrWhiteSpace(_gameStore.Player.Faction?.Name);
    public bool IsOpponentSelected => !string.IsNullOrWhiteSpace(_gameStore.Opponent.Faction?.Name);
    public bool IsMissionSelected => !string.IsNullOrWhiteSpace(_gameStore.Battleplan?.Name);

    private bool CanStartGame()
    {
        return (_gameStore?.Battleplan !=null && _gameStore?.Player?.Faction!=null && _gameStore?.Opponent?.Faction!=null);
    }

    [RelayCommand(CanExecute = nameof(CanStartGame))]
    private async Task StartGameAsync()
    {
        IsBusy = true;
        await _gameService.StartGame();
        await Shell.Current.GoToAsync(nameof(GamePage));
        await Shell.Current.GoToAsync($"{nameof(GamePage)}", true, new Dictionary<string, object>
        {
            { "CurrentRound", new BattleroundViewModel(_gameStore.Game.BattleRounds[0], _gameStore) }
        });
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