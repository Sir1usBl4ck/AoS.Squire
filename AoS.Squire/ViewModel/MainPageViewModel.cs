using System.Diagnostics;
using AoS.Squire.Services;
using AoS.Squire.Store;
using AoS.Squire.View;
using CommunityToolkit.Mvvm.Input;

namespace AoS.Squire.ViewModel;

public partial class MainPageViewModel : BaseViewModel
{
    private readonly GameStore _gameStore;
    private readonly GameService _gameService;

    public MainPageViewModel(GameStore gameStore, GameService gameService)
    {
        Title = "Aos Squire";
        _gameStore = gameStore;
        _gameService = gameService;
    }

    public string Version => AppInfo.Current.VersionString;

    [RelayCommand]
    private async Task NewGameAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            await _gameStore.GetDataAsync();
            _gameService.InitializeGame();
            await Shell.Current.GoToAsync($"{nameof(GameSetupPage)}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    
}