using System.Collections.ObjectModel;
using AoS.Squire.Model;
using AoS.Squire.Services;
using AoS.Squire.Store;
using AoS.Squire.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AoS.Squire.ViewModel;

public partial class FactionViewModel : BaseViewModel
{
    private readonly Faction _faction;
    private readonly LocalRepository _repository;

    public FactionViewModel(Faction faction, Player player, LocalRepository repository)
    {
        Player = player;
        _faction = faction;
        _repository = repository;
    }
    public Player Player { get; set; }
    public string FactionName => _faction.Name;

    public bool IsFavorite
    {
        get => _faction.IsFavorite;
        set => _faction.IsFavorite = value;
    }

    public bool IsNotFavorite => !IsFavorite;
    [RelayCommand]
    private async Task SelectFactionAsync()
    {
        Player.Faction = _faction;
        await Shell.Current.GoToAsync(nameof(GameSetupPage));

    }

    [RelayCommand]
    private async Task AddFavoriteAsync()
    {
       
            await _repository.AddFactionToFavorites(_faction.Id);
            IsFavorite = true;
        
            OnPropertyChanged(nameof(IsFavorite));
            OnPropertyChanged(nameof(IsNotFavorite));

    }

    [RelayCommand]
    private async Task RemoveFavoriteAsync()
    {
        await _repository.RemoveFromFavorites(_faction.Id);

        IsFavorite = false;

        OnPropertyChanged(nameof(IsFavorite));
        OnPropertyChanged(nameof(IsNotFavorite));
    }
}

[QueryProperty("Player", "Player")]
public partial class FactionSelectionViewModel : BaseViewModel
{
    private readonly GameStore _gameStore;
    private readonly LocalRepository _repository;
    private Player _player;

    public FactionSelectionViewModel(GameStore gameStore, LocalRepository repository)
    {
        _gameStore = gameStore;
        _repository = repository;
        Title = "Select A Faction";
    }

    public Player Player
    {
        get => _player;
        set
        {
            _player = value;
            LoadFactions();
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
            Factions.Add(new FactionViewModel(faction, Player, _repository));
        }
    }

    public ObservableCollection<FactionViewModel> Factions { get; set; } = new();
}