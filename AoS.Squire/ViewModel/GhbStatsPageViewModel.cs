using AoS.Squire.Model;
using AoS.Squire.Services;

namespace AoS.Squire.ViewModel;

public partial class GhbStatsPageViewModel : BaseViewModel
{
    private readonly ReportService _reportService;

    public GhbStatsPageViewModel(ReportService reportService)
    {
        _reportService = reportService;
        _reportService.DataLoaded += ReportService_DataLoaded;
    }

    public List<BattleplanStats> GhbStats => _reportService.GhbStats;

    private void ReportService_DataLoaded()
    {
        OnPropertyChanged(nameof(GhbStats));
    }
}