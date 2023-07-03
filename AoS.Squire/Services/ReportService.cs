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

    public int TotalNumberOfGames => GameReports.Count;
    public decimal Wins => GameReports.Count(r => r.PlayerScore > r.OpponentScore);
    public int Losses => GameReports.Count(r => r.PlayerScore < r.OpponentScore);
    public int Draws => GameReports.Count(r => r.PlayerScore == r.OpponentScore);

    public int WinRate => GetWinRate();

    private int GetWinRate()
    {
        if (TotalNumberOfGames==0)
        {
            return 0;
        }
        var result = (Wins / TotalNumberOfGames) * 100;
        return decimal.ToInt32(result);

    }

    public string MostWinsAgainstFactionName => GetMostWinsAgainstFactionName();
    public string MostLossesAgainstFactionName => GetMostLossesAgainstFactionName();

    private string GetMostLossesAgainstFactionName()
    {
        if (GameReports.Count == 0)
        {
            return string.Empty;
        }

        var losses = GameReports.Where(r => r.OpponentScore > r.PlayerScore)
            .ToDictionary(t => t.Id, t => t.OpponentFactionId);
        if (losses.Count==0)
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

        if (wins.Count==0)
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


    }


}
