using System.Collections.ObjectModel;
using AoS.Squire.Services;
using AoS.Squire.Store;
using AoS.Squire.View;
using CommunityToolkit.Mvvm.Input;

namespace AoS.Squire.ViewModel;

[QueryProperty("CurrentRound", "CurrentRound")]
public partial class GamePageViewModel : BaseViewModel
{
    private readonly GameStore _gameStore;
    private readonly LocalRepository _repository;

    public GamePageViewModel(GameStore gameStore, LocalRepository repository)
    {
        _gameStore = gameStore;
        _gameStore.GameScoreChanged += Game_ScoreChanged;
        _repository = repository;
        BattleRounds = new ObservableCollection<BattleroundViewModel>();
        foreach (var round in gameStore.Game.BattleRounds)
        {
            var vm = new BattleroundViewModel(round, gameStore);
            BattleRounds.Add(vm);
        }

    }

    private void Game_ScoreChanged()
    {
        OnPropertyChanged(nameof(PlayerScore));
        OnPropertyChanged(nameof(OpponentScore));
    }


    private BattleroundViewModel _currentRound;

    public BattleroundViewModel CurrentRound
    {
        get => _currentRound;

        set
        {
            if (_currentRound != value)
            {
                _currentRound = value;
                OnPropertyChanged();
            }

        }
    }


    [RelayCommand]
    private async Task EndGame()
    {
        IsBusy = true;

        var answer = await Application.Current.MainPage.DisplayAlert("End game", "Would your like to End the current Game?", "Yes, End Game", "No");
        if (answer)
        {
            await Shell.Current.GoToAsync("EndGamePage");
        }
        IsBusy = false;
    }

    [RelayCommand]
    private async Task PreviousRoundAsync()
    {
        IsBusy = true;
        var roundNumber = CurrentRound.BattleRoundNumber;
        if (roundNumber > 1)
        {
            await Shell.Current.GoToAsync($"{nameof(GamePage)}", true, new Dictionary<string, object>
            {
                { "CurrentRound", new BattleroundViewModel(_gameStore.Game.BattleRounds.FirstOrDefault(r=>r.RoundNumber==roundNumber-1),_gameStore)  }
            });
        }
        IsBusy = false;
    }
    [RelayCommand]
    private async Task NextRoundAsync()
    {
        IsBusy = true;
        var roundNumber = CurrentRound.BattleRoundNumber;

        if (roundNumber < 5)
        {
            await Shell.Current.GoToAsync($"{nameof(GamePage)}", true, new Dictionary<string, object>
            {
                { "CurrentRound", new BattleroundViewModel(_gameStore.Game.BattleRounds.FirstOrDefault(r=>r.RoundNumber==roundNumber+1),_gameStore) }
            });
        }

        IsBusy = false;
    }


    public string PlayerFactionName => _gameStore.Game.Player.Faction.Name;
    public string OpponentFactionName => _gameStore.Game.Opponent.Faction.Name;

    public int PlayerScore => _gameStore.Game.PlayerScore;
    public int OpponentScore => _gameStore.Game.OpponentScore;
    public ObservableCollection<BattleroundViewModel> BattleRounds { get; set; }


}