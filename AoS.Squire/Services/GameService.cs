using AoS.Squire.Model;

namespace AoS.Squire.Services;

public class GameService
{
    private HttpClient httpClient;

    public GameService()
    {
        httpClient = new HttpClient();
    }

    public async Task<List<Faction>> GetFactions()
    {
        httpClient.GetAsync()
    }
}