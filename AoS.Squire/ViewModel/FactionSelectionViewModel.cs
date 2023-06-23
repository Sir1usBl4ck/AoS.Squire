using System.Collections.ObjectModel;
using AoS.Squire.Model;
using AoS.Squire.Store;
using AoS.Squire.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AoS.Squire.ViewModel;

[QueryProperty("Player", "Player")]
public partial class FactionSelectionViewModel : BaseViewModel
{
    private readonly GameStore _gameStore;

    [ObservableProperty]
    private Player _player;

    public FactionSelectionViewModel(GameStore gameStore)
    {
        _gameStore = gameStore;
        Title = "Select A Faction";
        LoadFactions();
    }


    [RelayCommand]
    private async Task SelectionChanged(Faction faction)
    {
        if (faction != null)
        {
            await Shell.Current.GoToAsync(nameof(GameSetupPage));

        }
    }
    private void LoadFactions()
    {
        if (Factions.Count != 0)
        {
            Factions.Clear();
        }
        foreach (var faction in _gameStore.FilteredFactions)
        {
            Factions.Add(faction);
        }
    }

    public ObservableCollection<Faction> Factions { get; set; } = new();
}