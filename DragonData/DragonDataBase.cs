using Discord.WebSocket;
using DragonData.Context;
using Microsoft.EntityFrameworkCore;

namespace DragonData;

public class DragonDataBase : DataServer
{
    private readonly IDbContextFactory<DatabaseContext>_contextFactory;
    public DataBlocklist blocklist { get; set; }
    public DataWatcher watchlist { get; set; }

    public DragonDataBase(IDbContextFactory<DatabaseContext> dbContext, DataBlocklist dataBlocklist, DataWatcher watchlist) : base(dbContext)
    {
        _contextFactory = dbContext;
        blocklist = dataBlocklist;
        this.watchlist = watchlist;
    }

    public async Task AddWatcher(SocketGuild guild, SocketChannel channel, string tag)
    {
        using var context = _contextFactory.CreateDbContext();
        var query = context.Guilds.Where(x => x.guildID == guild.Id);
        if (await query.AnyAsync() != false) {
            await watchlist.CreateWatcher(channel, query, tag);
        }
    }



}
