using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DragonData.Module;

public class GuildModule
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public ulong guildID { get; set; }
    public string guildName { get; set; }

    public ICollection<BlockListModule> blockLists { get; set; }
}
