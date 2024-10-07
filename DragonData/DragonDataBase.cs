using DragonData.Context;
using Microsoft.EntityFrameworkCore;

namespace DragonData;

public class DragonDataBase
{
    private readonly IDbContextFactory<DatabaseContext>_contextFactory;
    public DataBlocklist blocklist { get; set; }

    public DragonDataBase(IDbContextFactory<DatabaseContext> dbContext, DataBlocklist dataBlocklist)
    {
        _contextFactory = dbContext;
        blocklist = dataBlocklist;
    }
}
