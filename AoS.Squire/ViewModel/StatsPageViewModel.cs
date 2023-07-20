using AoS.Squire.Model;
using AoS.Squire.Services;
using CommunityToolkit.Mvvm.Input;

namespace AoS.Squire.ViewModel;

public partial class StatsPageViewModel : BaseViewModel
{
    private readonly ReportService _reportService;

    public StatsPageViewModel(ReportService reportService)
    {
        _reportService = reportService;
    }

    public int TotalNumberOfGames => _reportService.GlobalStats.TotalNumberOfGames;
    public decimal Wins => _reportService.GlobalStats.Wins;
    public int Draws => _reportService.GlobalStats.Draws;
    public int Losses => _reportService.GlobalStats.Losses;
    public decimal WinRate => _reportService.GlobalStats.WinRate;
    public string MostWinsAgainstFactionName => _reportService.GlobalStats.MostWinsAgainstFactionName;
    public string MostLossesAgainstFactionName => _reportService.GlobalStats.MostLossesAgainstFactionName;


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
            "Yes Reset my Data", "Cancel");
        if (answer)
        {
            await _reportService.ResetDbAsync();
        }

        await LoadStatsAsync();
    }

}