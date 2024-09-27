

using System.ComponentModel.DataAnnotations;

namespace DragonData.Module;

public class BlockListModule
{
    [Key]
    public int blockID { get; set; }
    public ulong guildID { get; set; }
    public GuildModule Guild { get; set; }
    public string blockTag { get; set; }
}
