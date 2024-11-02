using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HornyDragonBot.Data.Module;

public class WatcherPostModule
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public ulong channelID { get; set; }
    public ulong guildID { get; set; }
    public GuildModule guild { get; set; }
    public string watchTags { get; set; }
    public bool pause { get; set; } = true;
    public int interval { get; set; } = 5;
}
