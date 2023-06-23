using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using AoS.Squire.Model;
using AoS.Squire.Services;
using AoS.Squire.Store;
using AoS.Squire.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AoS.Squire.ViewModel;

public partial class GamePageViewModel : BaseViewModel
{
    private readonly GameStore _gameStore;
    private readonly LocalRepository _repository;

    public GamePageViewModel(GameStore gameStore, LocalRepository repository)
    {
        _gameStore = gameStore;
        _repository = repository;
        BattleRounds = new ObservableCollection<BattleroundViewModel>();
        foreach (var round in gameStore.Game.BattleRounds)
        {
            var vm = new BattleroundViewModel(round, gameStore);
            BattleRounds.Add(vm);
            vm.ScoreChanged += BattleRound_ScoreChanged;
        }

    }

    private BattleroundViewModel _currentRound;

    public BattleroundViewModel CurrentRound
    {
        get => _currentRound;

        set
        {
            _currentRound = value;
            OnPropertyChanged();
        }
    }
    private void BattleRound_ScoreChanged()
    {
        OnPropertyChanged(nameof(PlayerScore));
        OnPropertyChanged(nameof(OpponentScore));
    }


    [RelayCommand]
    private async Task EndGame()
    {
        var answer = await Application.Current.MainPage.DisplayAlert("End game", "Would your like to End the current Game?", "Yes, End Game", "No");
        if (answer)
        {
            await Shell.Current.GoToAsync("EndGamePage");
        }
    }
    
    

    public string PlayerFactionName => _gameStore.Game.Player.Faction.Name;
    public string OpponentFactionName => _gameStore.Game.Opponent.Faction.Name;

    public int PlayerScore => _gameStore.Game.PlayerScore;
    public int OpponentScore => _gameStore.Game.OpponentScore;
    public ObservableCollection<BattleroundViewModel> BattleRounds { get; set; }




}