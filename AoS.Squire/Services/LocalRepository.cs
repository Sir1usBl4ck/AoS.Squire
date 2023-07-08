using AoS.Squire.Model;
using SQLite;

namespace AoS.Squire.Services;

public interface ILocalReportsRepository
{
    Task<List<GameReport>> GetGamesAsync();
    Task<List<BattleRoundReport>> GetRoundsAsync();
    Task<bool> DeleteAllAsync();
}

public class LocalRepository : ILocalReportsRepository
{
    private SQLiteAsyncConnection _database;

    public async Task<List<GameReport>> GetGamesAsync()
    {
        await Init();
        return await _database.Table<GameReport>().ToListAsync();

    }

    public async Task<List<BattleRoundReport>> GetRoundsAsync()
    {
        await Init();
        return await _database.Table<BattleRoundReport>().ToListAsync();
    }

    public async Task<bool> SaveGame(Game game)
    {
        var gameReport = CreateGameReport(game);
        var rounds = CreateBattleroundReport(game);
        await Init();

        await _database.InsertAsync(gameReport);
        await _database.InsertAllAsync(rounds);
        return true;

    }

    public async Task<bool> DeleteAllAsync()
    {
        await _database.DeleteAllAsync<BattleRoundReport>();
        await _database.DeleteAllAsync<GameReport>();
        return true;
    }

    async Task Init()
    {
        if (_database is not null)
            return;

        try
        {
            _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await _database.CreateTableAsync<GameReport>();
            await _database.CreateTableAsync<BattleRoundReport>();
            await _database.CreateTableAsync<FavoriteFaction>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private List<BattleRoundReport> CreateBattleroundReport(Game game)
    {
        var list = new List<BattleRoundReport>();
        foreach (var round in game.BattleRounds)
        {
            var roundReport = new BattleRoundReport
            {
                RoundNumber = round.RoundNumber,
                PlayerScore = round.PlayerTurn.Score,
                OpponentScore = round.OpponentTurn.Score,
                PlayerPriority = round.PlayerTurn.GoingFirst,
                OpponentPriority = round.OpponentTurn.GoingFirst,
            };
            list.Add(roundReport);
        }
        return list;
    }
    private GameReport CreateGameReport(Game game)
    {
        var gameReport = new GameReport();
        gameReport.PlayerFactionId = game.Player.Faction.Id;
        gameReport.OpponentFactionId = game.Opponent.Faction.Id;
        gameReport.PlayerBattleTacticsCount = game.BattleRounds
            .Select(r => r.PlayerTurn)
            .Count(t => t.VictoryPoints
                .Any(v => v.Description.Contains("tactic") && v.IsScored));
        gameReport.OpponentBattleTacticsCount = game.BattleRounds
             .Select(r => r.OpponentTurn)
             .Count(t => t.VictoryPoints
                 .Any(v => v.Description.Contains("tactic") && v.IsScored));

        gameReport.PlayerScore = game.PlayerScore;
        gameReport.OpponentScore = game.OpponentScore;

        return gameReport;
    }


    public async Task<List<int>> GetFavoriteFactionIds()
    {
        await Init();
        var favorites = await _database.Table<FavoriteFaction>().ToListAsync();
        return favorites.Select(f => f.FactionId).ToList();
    }

    public async Task<int> AddFactionToFavorites(int factionId)
    {
        return await _database.InsertAsync(new FavoriteFaction { FactionId = factionId });

    }

    public async Task RemoveFromFavorites(int factionId)
    {
        await Init();
        var faction = await _database.Table<FavoriteFaction>().Where(f => f.FactionId == factionId).FirstOrDefaultAsync();
        if (faction == null)
            throw new Exception($"Cannot delete. Faction not found for id {factionId}");
        await _database.DeleteAsync(faction);
    }
}
