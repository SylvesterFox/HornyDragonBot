

using Discord.WebSocket;
using DragonData.Context;
using DragonData.Module;
using Microsoft.EntityFrameworkCore;

namespace DragonData;

public class DragonDataBase
{
    private readonly IDbContextFactory<DatabaseContext>_contextFactory;

    private List<BlockListModule> blockLists;

    public DragonDataBase(IDbContextFactory<DatabaseContext> dbContext)
    {
        _contextFactory = dbContext;
    }


     public async Task<GuildModule> GetAndCreateGuild (SocketGuild Guild)
        {
            GuildModule itemGuild;
            using var context = _contextFactory.CreateDbContext();
            var query = context.Guilds.Where(x => x.guildID == Guild.Id);

            if (await query.AnyAsync() == false) {
                itemGuild = new GuildModule{
                    guildID = Guild.Id,
                    guildName = Guild.Name
                };
                context.Add(itemGuild);
                await context.SaveChangesAsync();
            } else {
                itemGuild = await query.FirstAsync();
            }
            
            return itemGuild;
        }

    public async Task SetBlocklistForGuild(SocketGuild guild, string block) {
        using var context = _contextFactory.CreateDbContext();
        BlockListModule blockList;

        GuildModule guildDB = await GetAndCreateGuild(guild);
        var query = context.blockLists.Where(x => x.blockTag == block);
        
        if (await query.AnyAsync() == false) {
            blockList = new BlockListModule{
                guildID = guildDB.guildID,
                blockTag = block
            };
            context.Add(blockList);
            await context.SaveChangesAsync();
        } else {
            return;
        }
    }


    public List<BlockListModule> GetGuildBlockList(SocketGuild guild) {
        
        using var context = _contextFactory.CreateDbContext();
        blockLists = context.blockLists.Where(x => x.guildID == guild.Id).ToList();
        
        return blockLists;
    }
}
