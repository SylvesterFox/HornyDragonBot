

namespace DragonData.Module;

public class GuildBlockListModule
{
    public ulong guildID { get; set; }
    public GuildModule Guild { get; set; }
    public string blockTag { get; set; }
}
