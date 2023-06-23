using AoS.Squire.Model;
using AoS.Squire.Services;
using AoS.Squire.Store;
using CommunityToolkit.Mvvm.Input;

namespace AoS.Squire.ViewModel;

public class TacticRecap
{
    public TacticRecap(int roundNumber, string tacticName)
    {
        RoundNumber = roundNumber;
        TacticName = tacticName;
    }

    public int RoundNumber { get; set; }
    public string TacticName { get; set; }
}

public partial class GameRecapViewModel : BaseViewModel
{
    private readonly Player _player;
    private readonly Game _game;
    private readonly List<Turn> _turns;

    public GameRecapViewModel(Player player, Game game, List<Turn> turns)
    {
        _player = player;
        _game = game;
        _turns = turns;
    }

    public int PlayerScore => _game.PlayerScore;
    public string PlayerFactionName => _player.Faction.Name;
    public int TookPriority => _turns.Select(t => t.GoingFirst).Count();

    public List<TacticRecap> TacticsRecap =>
        _turns.Select(t => new TacticRecap(t.RoundNumber, t.SelectedBattleTactic.Name)).ToList();
    public bool IsGranStrategyComplete
    {
        get => _player.IsGranStrategyCompleted;
        set
        {
            _player.IsGranStrategyCompleted = value; 
            OnPropertyChanged(nameof(PlayerScore));
        }
    }
}

public partial class EndGameViewModel : BaseViewModel
{
    private readonly LocalRepository _repository;
    private readonly GameStore _gameStore;

    public EndGameViewModel(LocalRepository repository, GameStore gameStore)
    {
        _repository = repository;
        _gameStore = gameStore;
        PlayerRecap = new GameRecapViewModel(gameStore.Player, gameStore.Game, gameStore.Game.BattleRounds.Select(b=>b.PlayerTurn).ToList());
        OpponentRecap = new GameRecapViewModel(gameStore.Opponent, gameStore.Game,gameStore.Game.BattleRounds.Select(b=>b.OpponentTurn).ToList());
    }

    public GameRecapViewModel PlayerRecap { get;}
    public GameRecapViewModel OpponentRecap { get; }




    [RelayCommand]
    private async Task Quit()
    {
        var answer = await Application.Current.MainPage.DisplayActionSheet("Would your like to Quit?", "Cancel", null, "Quit and Register Game", "Quit Without Saving");
        if (answer == "Quit and Register Game")
        {
            var result = await _repository.SaveGame(_gameStore.Game);
            if (result)
            {
                await Shell.Current.GoToAsync("//MainPage");

            }

        }

        if (answer == "Quit Without Saving")
        {
            await Shell.Current.GoToAsync("//MainPage");

        }
    }
}