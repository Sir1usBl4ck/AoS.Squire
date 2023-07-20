using System.ComponentModel;
using AoS.Squire.Model;
using AoS.Squire.Store;
using AoS.Squire.View;
using CommunityToolkit.Mvvm.Input;

namespace AoS.Squire.ViewModel;

public partial class TurnViewModel : BaseViewModel
{
    private readonly GameStore _store;
    public Turn Turn { get; }

    public TurnViewModel(Turn turn,int roundNumber, string playerTypeString,GameStore store)
    {
        _store = store;
        _store.PropertyChanged += Store_PropertyChanged;
        _store.GameScoreChanged += Game_ScoreChanged;
        Turn = turn;
        RoundNumber = roundNumber;
        VictoryPoints = turn.VictoryPoints.Select(v => new VictoryPointViewModel(v,store)).ToList();
      PlayerTypeString = playerTypeString;
    }

    private void Game_ScoreChanged()
    {
        OnPropertyChanged(nameof(Score));
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
    
    public bool HasExtraTracker => Turn.Player.Faction.HasExtraTracker;
    public string ExtraTrackerName => Turn.Player.Faction.ExtraTrackerName;
    
    public int ExtraTrackerValue
    {
        get => Turn.Player.ExtraTrackerValue;
        set
        {
            Turn.Player.ExtraTrackerValue = value;
            OnPropertyChanged();
        }
    }

    public int ExtraPoints
    {
        get => Turn.ExtraPoints;
        set
        {
            Turn.ExtraPoints = value; 
            OnPropertyChanged();
        }
    }
    public bool HasExtraPoints
    {
        get => Turn.HasExtraPoints;
        set
        {
            Turn.HasExtraPoints = value; 
            OnPropertyChanged();
        }
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
        set
        {
            Turn.GoingFirst = value; 
            OnPropertyChanged();
        }
    }

    public string SelectedBattleTacticName { get; set; } = "No battle tactic selected.";

    public string SelectedBattleTacticDescription { get; set; }
    

    public List<BattleTactic> PlayerTactics => Turn.Player.AvailableBattleTactics.ToList();

    public List<VictoryPointViewModel> VictoryPoints { get; set; }


    public void PopulateBattleTacticInfo(BattleTactic tactic)
    {
       SelectedBattleTacticDescription = tactic.Description;
       SelectedBattleTacticName = tactic.Name;
       IsTacticSelected = true;
       OnPropertyChanged(nameof(Score));
       OnPropertyChanged(nameof(SelectedBattleTacticDescription));
       OnPropertyChanged(nameof(SelectedBattleTacticName));
       OnPropertyChanged(nameof(IsTacticSelected));
       GoToScoreCommand.NotifyCanExecuteChanged();

    }
}