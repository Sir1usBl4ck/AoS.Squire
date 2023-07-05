using SQLite;

namespace AoS.Squire.Model;

public class FavoriteFaction
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int FactionId { get; set; }
}