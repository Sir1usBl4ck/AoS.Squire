using AoS.Squire.Model;
using AoS.Squire.Services;

namespace Aos.Tests.Services;

public class RemoteDataServiceMock : IRemoteDataService
{
    public Task<List<Faction>> GetFactions()
    {
        throw new NotImplementedException();
    }

    public Task<Ghb> GetGhb()
    {
        throw new NotImplementedException();
    }
}

public class LocalRepositoryMock : ILocalReportsRepository
{
    public async Task<List<GameReport>> GetGamesAsync()
    {
        var reports = new List<GameReport>();
        var report = new GameReport();
        report.OpponentBattleTacticsCount = 3;
        report.OpponentScore = 15;
        report.PlayerBattleTacticsCount = 4;
        report.PlayerScore = 20;
        reports.Add(report);
        return reports;

    }

    public Task<List<BattleRoundReport>> GetRoundsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAllAsync()
    {
        throw new NotImplementedException();
    }
}

public class ReportServiceTests
{
    [Fact]
    public async Task CreateGlobalStats_IsValid_WinRateIs50()
    {
        //arrange
        var mockLocalRepository = new LocalRepositoryMock();
        var mockRemoteDataService = new RemoteDataServiceMock();
        var reportService = new ReportService(mockLocalRepository, mockRemoteDataService);
        //act
        await reportService.GetData();
        var stats = reportService.GlobalStats;
        //assert

        Assert.Equal(100, stats.WinRate);


            
            



    }
}