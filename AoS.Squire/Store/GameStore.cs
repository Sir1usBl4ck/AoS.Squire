using AoS.Squire.Model;
using AoS.Squire.Services;
using AoS.Squire.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AoS.Squire.Store;

public partial class GameStore : BaseViewModel
{
    private readonly RemoteDataService _remoteDataService;
    private readonly LocalRepository _repository;

    public GameStore(RemoteDataService remoteDataService, LocalRepository repository)
    {
        _remoteDataService = remoteDataService;
        _repository = repository;
    }

    public Player Player { get; set; } = new();
    public Player Opponent { get; set; } = new();
        
    public void CalculateGameScore()
    {
        _game.CalculateScore();
        GameScoreChanged?.Invoke();
    }

    public event Action GameScoreChanged;
    
    [ObservableProperty] 
    private Battleplan _battleplan;
    
    private Game _game;
    private int _extraTrackerValue;

    public Game Game
    {
        get => _game;

        set
        {
            _game = value;
            OnPropertyChanged();
        }
    }


    public List<Faction> Factions { get; set; } = new();
    public Ghb Ghb { get; set; }
    public List<Faction> FilteredFactions { get; set; } = new();

    public int ExtraTrackerValue
    {
        get => _extraTrackerValue;
        set
        {
            if (value == _extraTrackerValue) return;
            _extraTrackerValue = value;
            OnPropertyChanged();
        }
    }


    public async Task GetDataAsync()
    {
        await GetFactionsAsync();
        await GetGhbAsync();
    }

    private async Task GetGhbAsync()
    {
        if (Ghb == null)
        {
            var ghb = await _remoteDataService.GetGhb();
            Ghb = ghb;
        }
    }

    private async Task GetFactionsAsync()
    {
        if (!Factions.Any())
        {
            var factions = await _remoteDataService.GetFactions();
            var favoriteIds = await _repository.GetFavoriteFactionIds();
            Factions.Clear();
            foreach (var faction in factions)
            {
                Factions.Add(faction);
                if (favoriteIds.Any(f=>f == faction.Id))
                {
                    faction.IsFavorite = true; 
                }
                
            }

        }
    }

    public void FilterFactions(string allianceName)
    {
        FilteredFactions.Clear();
        foreach (var faction in Factions)
        {
            if (faction.AllianceName.ToLower()==allianceName.ToLower())
            {
                FilteredFactions.Add(faction);
            }
        }
    }

    public async Task FilterFavoritesOnly()
    {
        FilteredFactions.Clear();
        var favorites = await _repository.GetFavoriteFactionIds();
        foreach (var faction in Factions.Where(faction => favorites.Contains(faction.Id)))
        {
            FilteredFactions.Add(faction);
        }
    }
}