using AoS.Squire.Model;

namespace AoS.Squire.Services;

public class ReportService
{
    private readonly ILocalReportsRepository _repository;
    private readonly IRemoteDataService _remoteDataService;

    public ReportService(ILocalReportsRepository repository, IRemoteDataService remoteDataService)
    {
        _repository = repository;
        _remoteDataService = remoteDataService;
    }

    public List<GameReport> GameReports { get; set; } = new();
    public List<BattleRoundReport> RoundReports { get; set; } = new();
    public List<Faction> Factions { get; set; } = new();
    public GlobalStats GlobalStats { get; set; }

    private GlobalStats CreateGlobalStats()
    {
        if (GameReports == null || !GameReports.Any())
        {
            throw new Exception(" Error creating Global Stats.  GameReports can't be empty");
        }
        var stats = new GlobalStats();
        stats.WinRate = GameReports.Count(r => r.PlayerScore > r.OpponentScore);
        stats.Losses = GameReports.Count(r => r.PlayerScore < r.OpponentScore);
        stats.Draws = GameReports.Count(r => r.PlayerScore == r.OpponentScore);
        stats.TotalNumberOfGames = GameReports.Count;
        stats.WinRate = GetWinRate(stats);
        stats.MostWinsAgainstFactionName = GetMostWinsAgainstFactionName();
        stats.MostLossesAgainstFactionName = GetMostLossesAgainstFactionName();
        return stats;

    }

    private int GetWinRate(GlobalStats stats)
    {
        if (stats.TotalNumberOfGames == 0)
        {
            return 0;
        }
        var result = (stats.Wins / stats.TotalNumberOfGames) * 100;
        return decimal.ToInt32(result);

    }



    private string GetMostLossesAgainstFactionName()
    {
        if (GameReports.Count == 0)
        {
            return string.Empty;
        }

        var losses = GameReports.Where(r => r.OpponentScore > r.PlayerScore)
            .ToDictionary(t => t.Id, t => t.OpponentFactionId);
        if (losses.Count == 0)
        {
            return "Not enough Data.";
        }

        var lossesCount = losses.Values.GroupBy(value => value)
            .Select(group => new { FactionId = group.Key, Count = group.Count() })
            .OrderByDescending(item => item.Count);

        var mostLossesAgainstId = lossesCount.FirstOrDefault().FactionId;
        return Factions.FirstOrDefault(f => f.Id == mostLossesAgainstId)?.Name;
    }

    public string GetMostWinsAgainstFactionName()
    {
        if (GameReports.Count == 0)
        {
            return string.Empty;
        }
        var wins = GameReports.Where(r => r.PlayerScore > r.OpponentScore)
            .ToDictionary(t => t.Id, t => t.OpponentFactionId);

        if (wins.Count == 0)
        {
            return "Not enough Data.";
        }

        var winsCounts = wins.Values.GroupBy(value => value)
            .Select(group => new { FactionId = group.Key, Count = group.Count() })
            .OrderByDescending(item => item.Count);

        var mostWinsAgainstId = winsCounts.FirstOrDefault().FactionId;
        return Factions.FirstOrDefault(f => f.Id == mostWinsAgainstId)?.Name;




    }

    public async Task ResetDbAsync()
    {
        await _repository.DeleteAllAsync();
    }


    public async Task GetData()
    {
        Factions = await _remoteDataService.GetFactions();
        GameReports = await _repository.GetGamesAsync();
        RoundReports = await _repository.GetRoundsAsync();
        GlobalStats = CreateGlobalStats();


    }

}
