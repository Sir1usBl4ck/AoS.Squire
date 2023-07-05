using AoS.Squire.Model;
using AoS.Squire.Store;
using AoS.Squire.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AoS.Squire.ViewModel;

[QueryProperty("Player", "Player")]
public partial class AllianceSelectionViewModel : BaseViewModel
{
    private readonly GameStore _gameStore;
    [ObservableProperty] private Player _player;

    public AllianceSelectionViewModel(GameStore gameStore)
    {
        _gameStore = gameStore;
    }
    [RelayCommand]
    private async Task SelectAllianceAsync(string allianceName)
    {
        if (string.IsNullOrEmpty(allianceName))
        {
            return;
        }
        _gameStore.FilterFactions(allianceName);
        await GoToFactionSelectionPage();
        ;
    }

    [RelayCommand]
    private async Task GoToMyArmiesAsync()
    {
        await _gameStore.FilterFavoritesOnly();
        await GoToFactionSelectionPage();
    }

    private async Task GoToFactionSelectionPage()
    {
        await Shell.Current.GoToAsync($"{nameof(FactionSelectionPage)}", true, new Dictionary<string, object>
        {
            { "Player", Player }
        });
    }

}