using System.Collections.ObjectModel;
using AoS.Squire.Model;
using AoS.Squire.Store;

namespace AoS.Squire.ViewModel;

public class BattleroundViewModel : BaseViewModel
{
    private readonly BattleRound _battleRound;

    public BattleroundViewModel(BattleRound battleRound,GameStore store)
    {
        _battleRound = battleRound;
        
        PlayerTurn = new TurnViewModel(battleRound.PlayerTurn,battleRound.RoundNumber,"player",store);
        PlayerTurn.ScoreChanged += Turn_ScoreChanged;
        OpponentTurn = new TurnViewModel(battleRound.OpponentTurn,battleRound.RoundNumber,"opponent",store);
        OpponentTurn.ScoreChanged += Turn_ScoreChanged;


    }

    private void Turn_ScoreChanged()
    {
        OnScoreChanged();
    }

    public int BattleRoundNumber=> _battleRound.RoundNumber;

    public TurnViewModel PlayerTurn { get; set; }
    public TurnViewModel OpponentTurn { get; set; }
    
    public event Action ScoreChanged;

    protected virtual void OnScoreChanged()
    {
        ScoreChanged?.Invoke();
    }
}