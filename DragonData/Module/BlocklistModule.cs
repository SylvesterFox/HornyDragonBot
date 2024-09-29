
using System.ComponentModel.DataAnnotations;

namespace DragonData.Module;

public class BlocklistModule
{
    [Key]
    public int blockID { get; set; }
    public ulong userID { get; set; }
    public UserModule User { get; set; }
    public string blockTag { get; set; }

}
