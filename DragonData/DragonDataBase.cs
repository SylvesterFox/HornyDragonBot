using DragonData.Base;
using DragonData.Context;
using Microsoft.EntityFrameworkCore;

namespace DragonData;

public class DragonDataBase
{

    public DataGuild dataGuild { get; set; }
    public DataBlocklist blocklist { get; set; }
    public DataWatcher watchlist { get; set; }
    public DataUser dataUser { get; set; }

    public DragonDataBase(IDbContextFactory<DatabaseContext> dbContext, 
        DataBlocklist dataBlocklist, 
        DataWatcher dataWatchlist,
        DataGuild guildData,
        DataUser userData)
    {
        blocklist = dataBlocklist;
        watchlist = dataWatchlist;
        dataGuild = guildData;
        dataUser = userData;
    }



   

    
}
