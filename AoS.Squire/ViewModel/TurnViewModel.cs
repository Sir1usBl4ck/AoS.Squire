using System.Collections.ObjectModel;
using System.ComponentModel;
using AoS.Squire.Model;
using AoS.Squire.Store;
using AoS.Squire.View;
using CommunityToolkit.Mvvm.Input;

namespace AoS.Squire.ViewModel;

public class VictoryPointViewModel : BaseViewModel
{
    private readonly VictoryPoint _victoryPoint;

    public VictoryPointViewModel(VictoryPoint victoryPoint)
    {
        _victoryPoint = victoryPoint;
    }

    public event Action ScoreChanged;
    public string Description => _victoryPoint.Description;
    public int Points => _victoryPoint.Points;
    public bool IsScored
    {
        get => _victoryPoint.IsScored;
        set
        {
            _victoryPoint.IsScored = value;
            OnPropertyChanged();
            ScoreChanged?.Invoke();
        }
    }
}
public partial class TurnViewModel : BaseViewModel
{
    private readonly GameStore _store;
    public Turn Turn { get; }
    public event Action ScoreChanged;

    public TurnViewModel(Turn turn,int roundNumber, string playerTypeString,GameStore store)
    {
        _store = store;
        _store.PropertyChanged += Store_PropertyChanged;
        Turn = turn;
        RoundNumber = roundNumber;
        VictoryPoints = turn.VictoryPoints.Select(v => new VictoryPointViewModel(v)).ToList();
        foreach (var pointViewModel in VictoryPoints)
        {
            pointViewModel.ScoreChanged += VictoryPoint_ScoreChanged;
        }
        PlayerTypeString = playerTypeString;
    }

    [RelayCommand]
    private async Task GoToTacticsPicker(TurnViewModel turn)
    {
        await Shell.Current.GoToAsync($"{nameof(TacticsPickerPage)}", true, new Dictionary<string, object>
        {
            {"Turn",this}
        });
    }

    
    [RelayCommand(CanExecute = nameof(CanGoToScore))]
    private async Task GoToScoreAsync(TurnViewModel turn)
    {
        await Shell.Current.GoToAsync(nameof(TurnPage), true, new Dictionary<string, object>
        {
            {"Turn",turn}
        });


    }

    private bool CanGoToScore()
    {
        return IsTacticSelected;
    }

    private void Store_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName==nameof(_store.ExtraTrackerValue))
        {
            OnPropertyChanged(nameof(ExtraTrackerValue));
        }
    }

    private void VictoryPoint_ScoreChanged()
    {
       CalculateScore();
       ScoreChanged?.Invoke();
    }

    public bool HasExtraTracker => Turn.Player.Faction.HasExtraTracker;
    public string ExtraTrackerName => Turn.Player.Faction.ExtraTrackerName;

    public int ExtraTrackerValue
    {
        get => _store.ExtraTrackerValue;
        set => _store.ExtraTrackerValue = value;
    }


    public bool IsTacticSelected { get; set; }
    public string PlayerTypeString { get; set; }
    public string PlayerName => Turn.Player.Name;
    public string PlayerFaction => Turn.Player.Faction.Name;
    public int Score => Turn.Score;
    public int RoundNumber { get; set; }
    public bool GoingFirst
    {
        get => Turn.GoingFirst;
        set => Turn.GoingFirst = value;
    }

    public string SelectedBattleTacticName { get; set; } = "No battle tactic selected.";

    public string SelectedBattleTacticDescription { get; set; }
    

    public List<BattleTactic> PlayerTactics => Turn.Player.AvailableBattleTactics.ToList();

    public List<VictoryPointViewModel> VictoryPoints { get; set; }


    public void CalculateScore()
    {
        OnPropertyChanged(nameof(Score));
    }

    public void PopulateBattleTacticInfo(BattleTactic tactic)
    {
       SelectedBattleTacticDescription = tactic.Description;
       SelectedBattleTacticName = tactic.Name;
       IsTacticSelected = true;
       CalculateScore();
       OnPropertyChanged(nameof(SelectedBattleTacticDescription));
       OnPropertyChanged(nameof(SelectedBattleTacticName));
       OnPropertyChanged(nameof(IsTacticSelected));
       GoToScoreCommand.NotifyCanExecuteChanged();

    }
}