using AoS.Squire.Model;
using AoS.Squire.Store;

namespace AoS.Squire.ViewModel;

public class BattleroundViewModel : BaseViewModel
{
    private readonly BattleRound _battleRound;

    public BattleroundViewModel(BattleRound battleRound, GameStore store)
    {
        _battleRound = battleRound;

        PlayerTurn = new TurnViewModel(battleRound.PlayerTurn, battleRound.RoundNumber, "player", store);
        InitializeTurn(_battleRound.PlayerTurn, PlayerTurn);
        OpponentTurn = new TurnViewModel(battleRound.OpponentTurn, battleRound.RoundNumber, "opponent", store);
        InitializeTurn(_battleRound.OpponentTurn, OpponentTurn);


        Turns = new List<TurnViewModel>
        {
            PlayerTurn,
            OpponentTurn
        };
    }

    private void InitializeTurn(Turn turn, TurnViewModel viewModel)
    {
        if (turn.SelectedBattleTactic != null)
        {
            viewModel.PopulateBattleTacticInfo(turn.SelectedBattleTactic);
        }

    }


    public int BattleRoundNumber => _battleRound.RoundNumber;

    public List<TurnViewModel> Turns { get; set; }

    public TurnViewModel PlayerTurn { get; set; }
    public TurnViewModel OpponentTurn { get; set; }

}