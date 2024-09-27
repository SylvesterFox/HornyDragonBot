using System.ComponentModel.DataAnnotations;

namespace DragonData.Module;

public class GuildModule
{
    [Key]
    public ulong guildID { get; set; }
    public string guildName { get; set; }

    public ICollection<BlockListModule> blockLists { get; set; }
}
