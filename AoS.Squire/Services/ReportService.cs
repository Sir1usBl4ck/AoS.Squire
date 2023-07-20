using AoS.Squire.Model;

namespace AoS.Squire.Services;

public class ReportService
{
    private readonly LocalRepository _repository;
    private readonly RemoteDataService _remoteDataService;

    public ReportService(LocalRepository repository, RemoteDataService remoteDataService)
    {
        _repository = repository;
        _remoteDataService = remoteDataService;
    }

    public List<GameReport> GameReports { get; set; } = new();
    public List<BattleRoundReport> RoundReports { get; set; } = new();
    public List<Faction> Factions { get; set; } = new();
    public GlobalStats GlobalStats { get; set; } = new();
    public List<FactionStats> FactionsStatsList { get; set; } = new();
    public List<BattleplanStats> GhbStats { get; set; }
    public Ghb LastGhb { get; set; }
    public event Action DataLoaded;


    private List<BattleplanStats> CreateGhbStats()
    {
        var list = new List<BattleplanStats>();
        var battleplans = LastGhb.Missions.Select(m => m.Name).Distinct().ToList();
        foreach (var battleplan in battleplans)
        {
            var reports = GameReports.Where(r => r.Battleplan == battleplan).ToList();
            var planStats = new BattleplanStats();
            planStats.BattleplanName = battleplan;
            planStats.Wins = GetWins(reports);
            planStats.Draw = GetDraws(reports);
            planStats.Losses = GetLosses(reports);
            planStats.TotalNumberOfGames = reports.Count();
            planStats.WinRate = GetWinRate(planStats.Wins, planStats.TotalNumberOfGames);
            planStats.AverageCompletedTactics = planStats.TotalNumberOfGames != 0 
                ? (decimal)reports.Sum(r => r.PlayerBattleTacticsCount) / planStats.TotalNumberOfGames
                : 0;

            list.Add(planStats);
        }

        return list;
    }


    private List<FactionStats> CreateFactionStats()
    {
        var list = new List<FactionStats>();

        foreach (var faction in Factions)
        {
            var factionReports = GameReports.Where(r => r.PlayerFactionId == faction.Id).ToList();
            if (factionReports.Count==0)
            {
                continue;
            }
            var stats = new FactionStats();
            stats.FactionName = faction.Name;
            stats.Wins = GetWins(factionReports);
            stats.Losses = GetLosses(factionReports);
            stats.Draws = GetDraws(factionReports);
            stats.NumberOfGames = factionReports.Count;
            stats.WinRate = GetWinRate(stats.Wins, stats.NumberOfGames);
            stats.MostLossesAgainstFactionName = GetMostLossesAgainstFactionName(factionReports);
            stats.MostWinsAgainstFactionName = GetMostWinsAgainstFactionName(factionReports);

            list.Add(stats);
        }

        return list;
    }

    private GlobalStats CreateGlobalStats()
    {
        if (GameReports == null || !GameReports.Any())
        {
            return new GlobalStats();
        }
        var stats = new GlobalStats();
        stats.Wins = GetWins(GameReports);
        stats.Losses = GetLosses(GameReports);
        stats.Draws = GetDraws(GameReports);
        stats.TotalNumberOfGames = GameReports.Count;
        stats.WinRate = GetWinRate(stats.Wins, stats.TotalNumberOfGames);
        stats.MostWinsAgainstFactionName = GetMostWinsAgainstFactionName(GameReports);
        stats.MostLossesAgainstFactionName = GetMostLossesAgainstFactionName(GameReports);
        return stats;

    }

    private int GetWins(IEnumerable<GameReport> reports)
    {
        return reports.Where(r => r.PlayerScore != 0 && r.OpponentScore != 0).Count(r => r.PlayerScore > r.OpponentScore);
    }
    private int GetLosses(IEnumerable<GameReport> reports)
    {
        return reports.Where(r => r.PlayerScore != 0 && r.OpponentScore != 0).Count(r => r.PlayerScore < r.OpponentScore);
    }
    private int GetDraws(IEnumerable<GameReport> reports)
    {
        return reports.Where(r => r.PlayerScore != 0 && r.OpponentScore != 0).Count(r => r.PlayerScore == r.OpponentScore);
    }

    private int GetWinRate(int wins, int totalNumberOfGames)
    {
        if (totalNumberOfGames == 0)
        {
            return 0;
        }
        var result = (wins / totalNumberOfGames) * 100;
        return decimal.ToInt32(result);

    }



    private string GetMostLossesAgainstFactionName(List<GameReport> gameReports)
    {
        if (!gameReports.Any())
        {
            return string.Empty;
        }

        var losses = gameReports.Where(r => r.OpponentScore > r.PlayerScore)
            .ToDictionary(t => t.Id, t => t.OpponentFactionId);
        if (losses.Count == 0)
        {
            return "Not enough Data.";
        }

        var lossesCount = losses.Values.GroupBy(value => value)
            .Select(group => new { FactionId = group.Key, Count = group.Count() })
            .OrderByDescending(item => item.Count).ToList();

        var mostLossesAgainstId = lossesCount.FirstOrDefault().FactionId;
        return Factions.FirstOrDefault(f => f.Id == mostLossesAgainstId)?.Name;
    }

    public string GetMostWinsAgainstFactionName(List<GameReport> gameReports)
    {
        if (!gameReports.Any())
        {
            return string.Empty;
        }
        var wins = gameReports.Where(r => r.PlayerScore > r.OpponentScore)
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
        LastGhb = await _remoteDataService.GetGhb();
        GlobalStats = CreateGlobalStats();
        FactionsStatsList = CreateFactionStats();
        GhbStats = CreateGhbStats();

        DataLoaded?.Invoke();

    }

}
