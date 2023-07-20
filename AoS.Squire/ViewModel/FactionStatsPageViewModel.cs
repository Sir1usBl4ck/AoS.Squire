using AoS.Squire.Model;
using AoS.Squire.Services;

namespace AoS.Squire.ViewModel;

public partial class FactionStatsPageViewModel : BaseViewModel
{
    private readonly ReportService _reportService;

    public FactionStatsPageViewModel(ReportService reportService)
    {
        _reportService = reportService;
        _reportService.DataLoaded += ReportService_DataLoaded;
    }
    private void ReportService_DataLoaded()
    {
        OnPropertyChanged(nameof(FactionStats));
    }

    public List<FactionStats> FactionStats => _reportService.FactionsStatsList;
    
}