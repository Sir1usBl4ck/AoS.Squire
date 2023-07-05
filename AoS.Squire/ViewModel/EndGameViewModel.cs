using AoS.Squire.Model;
using AoS.Squire.Services;
using AoS.Squire.Store;
using CommunityToolkit.Mvvm.Input;

namespace AoS.Squire.ViewModel;

public class TacticRecap
{
    public TacticRecap(int roundNumber, string tacticName, bool isComplete)
    {
        RoundNumber = roundNumber;
        TacticName = tacticName;
        IsComplete = isComplete;
    }

    public int RoundNumber { get; set; }
    public bool IsComplete { get; set; }
    public string TacticName { get; set; }
}

public partial class GameRecapViewModel : BaseViewModel
{
    private readonly Player _player;
    private readonly Game _game;

    public GameRecapViewModel(Player player, Game game, List<Turn> turns)
    {
        _player = player;
        _game = game;
        TacticsRecap = GetTacticsRecap(turns);
        TookPriority = CalculateGoingFirstTurns(turns);

    }

    public int PlayerScore => _player.IsOpponent? _game.OpponentScore : _game.PlayerScore;
    public string PlayerName => _player.Name;
    public string PlayerFactionName => _player.Faction.Name;
    public int TookPriority { get; set; }

    public List<TacticRecap> TacticsRecap { get; set; }

    private int CalculateGoingFirstTurns(List<Turn> turns)
    {
        var result = turns.Count(t => t.GoingFirst);
        return result;
    }

    private bool HasCompletedTactic(Turn turn)
    {
        return turn.VictoryPoints.Where(v => v.IsScored)
            .FirstOrDefault(v => v.Description.ToLower().Contains("tactic".ToLower())) != null;
    }

    private List<TacticRecap> GetTacticsRecap(List<Turn> turns )
    {
        var result = turns
            .Select(t =>
                new TacticRecap(t.RoundNumber,
                    (t.SelectedBattleTactic!=null ? t.SelectedBattleTactic.Name : "-"),
                    HasCompletedTactic(t) ))
            .ToList();
        var orderedResult = result.OrderBy(t => t.RoundNumber);
        return orderedResult.ToList();
    }

    public int NumberOfTactics => TacticsRecap.Count(t => (t.TacticName!="-" && t.IsComplete));
    public bool IsGranStrategyComplete
    {
        get => _player.IsGranStrategyCompleted;
        set
        {
            _player.IsGranStrategyCompleted = value; 
            _game.CalculateScore();
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