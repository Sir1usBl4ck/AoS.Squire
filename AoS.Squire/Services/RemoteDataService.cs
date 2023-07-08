using System.Diagnostics;
using System.Net.Http.Json;
using AoS.Squire.Model;

namespace AoS.Squire.Services;

public interface IRemoteDataService
{
    Task<List<Faction>> GetFactions();
    Task<Ghb> GetGhb();
}

public class RemoteDataService : IRemoteDataService
{
    private HttpClient httpClient;

    public RemoteDataService()
    {
        httpClient = new HttpClient();
    }

    public async Task<List<Faction>> GetFactions()
    {
        try
        {
            var factions = new List<Faction>();
            var url = "https://sir1usbl4ck.github.io/AoS.Squire/factions.json";
            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                await Shell.Current.DisplayAlert("Error!", "Unable to download factions files", "OK");
                return factions;

            }
            var tactics = await response.Content.ReadFromJsonAsync<List<TacticsDto>>();

            var groupedFactions = tactics.DistinctBy(t=>t.FactionId).Select(t => new Faction()
                { Id = t.FactionId, Name = t.FactionName, AllianceName = t.AllianceName, HasExtraTracker = t.HasExtraTracker, ExtraTrackerName = t.ExtraTrackerName}).ToList();
            foreach (var faction in groupedFactions)
            {
                faction.BattleTactics = new List<BattleTactic>();

            }
            foreach (var tacticsDto in tactics)
            {
                var tactic = new BattleTactic() { Description = tacticsDto.Description, Name = tacticsDto.Name };
                var selectedFaction = groupedFactions.First(f=>f.Id==tacticsDto.FactionId);
                selectedFaction.BattleTactics.Add(tactic);
            }



            factions = groupedFactions;



            return factions;
        }
        catch (Exception e)
        {
            await Shell.Current.DisplayAlert("Error!", "An Error occurred while trying to download source files", "OK");

            Debug.WriteLine(e);
            throw;
        }
       

    }

    public async Task<Ghb> GetGhb()
    {
        try
        {
            var ghb = new Ghb();
            var url = "https://sir1usbl4ck.github.io/AoS.Squire/ghb.json";
            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                ghb = await response.Content.ReadFromJsonAsync<Ghb>();
            }
        
            return ghb;
        }
        catch (Exception e)
        {
            await Shell.Current.DisplayAlert("Error!", "Unable to download Ghb files", "OK");

            Debug.WriteLine(e);
            throw;
        }
    }
}