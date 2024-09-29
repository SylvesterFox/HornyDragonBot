
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DragonData.Module;

public class UserModule
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public ulong userID { get; set; }
    public string username { get; set; }
    public ICollection<BlocklistModule> Blocklists { get; set; }
}
