using AoS.Squire.Model;
using AoS.Squire.Services;
using CommunityToolkit.Mvvm.Input;

namespace AoS.Squire.ViewModel;

public partial class StatsPageViewModel : BaseViewModel
{
    private readonly ReportService _reportService;
    private readonly GlobalStats _globalStats;

    public StatsPageViewModel(ReportService reportService)
    {
        _reportService = reportService;
        _globalStats = reportService.GlobalStats;
    }

    public int TotalNumberOfGames => _globalStats.TotalNumberOfGames;
    public decimal Wins => _globalStats.Wins;
    public int Draws => _globalStats.Draws;
    public int Losses => _globalStats.Losses;
    public decimal WinRate => _globalStats.WinRate;
    public string MostWinsAgainstFactionName => _globalStats.MostWinsAgainstFactionName;
    public string MostLossesAgainstFactionName => _globalStats.MostLossesAgainstFactionName;


    [RelayCommand]
    private async Task LoadStatsAsync()
    {
        IsBusy = true;
        await _reportService.GetData();
        OnPropertyChanged(nameof(TotalNumberOfGames));
        OnPropertyChanged(nameof(Wins));
        OnPropertyChanged(nameof(Losses));
        OnPropertyChanged(nameof(Draws));
        OnPropertyChanged(nameof(WinRate));
        OnPropertyChanged(nameof(MostLossesAgainstFactionName));
        OnPropertyChanged(nameof(MostWinsAgainstFactionName));
        IsBusy = false;

    }
    [RelayCommand]
    private async Task ResetStatsAsync()
    {
        var answer = await Application.Current.MainPage.DisplayAlert("Attention!", "You are about to reset the database. All your data will be lost. Are you sure?",
            "Cancel", "Yes Reset my Data");
        if (answer)
        {
            await _reportService.ResetDbAsync();
        }

        await LoadStatsAsync();
    }

}