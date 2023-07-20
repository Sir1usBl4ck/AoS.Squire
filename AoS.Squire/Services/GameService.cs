using AoS.Squire.Model;
using AoS.Squire.Store;

namespace AoS.Squire.Services;

public class GameService
{
    private readonly GameStore _gameStore;
    private Game _game;

    public GameService(GameStore gameStore)
    {
        _gameStore = gameStore;
    }
    public void InitializeGame()
    {
        var player = new Player(){Name = "Player"};
        var opponent = new Player(){Name = "Opponent", IsOpponent = true};
        _gameStore.Player = player;
        _gameStore.Opponent = opponent;
        
    }
    public async Task StartGame()
    {
        _gameStore.Game = new Game( _gameStore.Player, _gameStore.Opponent,_gameStore.Battleplan);
        _game = _gameStore.Game;
        InitializeBattleRounds(5);

        InjectTactics(_gameStore.Player);
        InjectTactics(_gameStore.Opponent);

        InjectVictoryPoints(_gameStore.Battleplan);

    }

   
    public void SelectTactic(Turn turn, BattleTactic tactic)
    {
        turn.SelectedBattleTactic = tactic;
        var usedTactics = new List<BattleTactic>();
        usedTactics.Clear();
        if (turn.Player.IsOpponent)
        {
            usedTactics.AddRange(_game.BattleRounds.Select(r => r.OpponentTurn).Select(t => t.SelectedBattleTactic).ToList());
        }
        else
        {
            usedTactics.AddRange(_game.BattleRounds.Select(r => r.PlayerTurn).Select(t => t.SelectedBattleTactic).ToList());
        }

        foreach (var battleTactic in turn.Player.AvailableBattleTactics.Except(usedTactics))
        {
            battleTactic.IsAvailable = true;
        }

        tactic.IsAvailable = false;
    }
    private void InitializeBattleRounds(int amount)
    {
        int id = 0;
        for (int i = 1; i <= amount; i++)
        {
            var battleRound = new BattleRound()
            {
                RoundNumber = i
            };
            
            battleRound.PlayerTurn = new Turn(id, battleRound, _game.Player)
            {
                GoingFirst = true
            };
            id++;

            battleRound.OpponentTurn = new Turn(id, battleRound, _game.Opponent);
            id++;

            _game.BattleRounds.Add(battleRound);

        }
    }

    private void InjectTactics(Player player)
    {
        player.AvailableBattleTactics.Clear();
        foreach (var tactic in _gameStore.Ghb.BattleTactics)
        {
            player.AvailableBattleTactics.Add(new BattleTactic { Description = tactic.Description, Name = tactic.Name, IsAvailable = true });
        }

        foreach (var tactic in player.Faction.BattleTactics)
        {
            tactic.IsAvailable = true;
            player.AvailableBattleTactics.Add(new BattleTactic { Description = tactic.Description, Name = tactic.Name, IsAvailable = true });
        }
    }

    private void InjectVictoryPoints(Battleplan battleplan)
    {
        if (battleplan == null) return;
        foreach (var gameBattleRound in _game.BattleRounds)
        {
            foreach (var victoryPoint in battleplan.VictoryPoints)
            {
                gameBattleRound.PlayerTurn.VictoryPoints.Add(new VictoryPoint() { Description = victoryPoint.Description, Points = victoryPoint.Points });
                gameBattleRound.OpponentTurn.VictoryPoints.Add(new VictoryPoint() { Description = victoryPoint.Description, Points = victoryPoint.Points });
            }
        }
    }


   
}