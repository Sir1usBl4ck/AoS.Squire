﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace AoS.Squire.Model;

public class Game
{
    public Game(Player player, Player opponent)
    {
        Player = player;
        Opponent = opponent;
    }
    public Player Player { get; }
    public Player Opponent { get; }
    public List<BattleRound> BattleRounds { get; } = new();

    public int PlayerScore => BattleRounds.Select(r => r.PlayerTurn).Select(p => p.Score).Sum() + (Player.IsGranStrategyCompleted ? 3 : 0);

    public int OpponentScore => BattleRounds.Select(r => r.OpponentTurn).Select(p => p.Score).Sum()+ (Opponent.IsGranStrategyCompleted ? 3 : 0);

}