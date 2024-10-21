using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DragonData.Module;

public class GuildModule
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public ulong guildID { get; set; }
    public string guildName { get; set; }
    public string queryCategoryName { get; set; } = "HorryDragonBot";
    public ulong queryCatagoryId { get; set; } = 0;

    public ICollection<GuildBlockListModule> GuildblockLists { get; set; }
    public ICollection<WatcherPostModule> WatcherPost { get; set;}
}
