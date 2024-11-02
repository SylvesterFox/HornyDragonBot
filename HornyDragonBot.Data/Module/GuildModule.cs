using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HornyDragonBot.Data.Module;

public class GuildModule
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public ulong guildID { get; set; }
    public string guildName { get; set; }
    public ulong queryCatagoryId { get; set; } = 0;

    public ICollection<GuildBlockListModule> GuildblockLists { get; set; }
    public ICollection<WatcherPostModule> WatcherPost { get; set;}
}
