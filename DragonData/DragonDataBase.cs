using Discord.WebSocket;
using DragonData.Context;
using DragonData.Module;
using Microsoft.EntityFrameworkCore;

namespace DragonData;

public class DragonDataBase
{
    private readonly IDbContextFactory<DatabaseContext>_contextFactory;


    public DragonDataBase(IDbContextFactory<DatabaseContext> dbContext)
    {
        _contextFactory = dbContext;
    }


     public async Task<GuildModule> GetAndCreateGuild(SocketGuild Guild)
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

    public async Task<UserModule> GetAndCreateUser(SocketUser user) {
        UserModule itemUser;
        using var context = _contextFactory.CreateDbContext();
        var query = context.Users.Where(x => x.userID == user.Id);

        if (await query.AnyAsync() == false) {
            itemUser = new UserModule{
                userID = user.Id,
                username = user.Username
            };
            context.Add(itemUser);
            await context.SaveChangesAsync();
        } else {
            itemUser = await query.FirstAsync();
        }

        return itemUser;
    }

    public async Task SetBlocklist(SocketUser user, string blockTag) {
        using var context = _contextFactory.CreateDbContext();
        BlocklistModule blocklist;

        UserModule userDB = await GetAndCreateUser(user);
        var query = context.blocklists.Where(x => x.blockTag == blockTag);

        if (await query.AnyAsync() == false) {
            blocklist = new BlocklistModule {
                userID = userDB.userID,
                blockTag = blockTag
            };
            context.Add(blocklist);
        } else {
            return;
        }

        await context.SaveChangesAsync();
    }

    public async Task SetBlocklistForGuild(SocketGuild guild, string block) {
        using var context = _contextFactory.CreateDbContext();
        GuildBlockListModule blockList;

        GuildModule guildDB = await GetAndCreateGuild(guild);
        var query = context.GuildBlockLists.Where(x => x.blockTag == block);
        
        if (await query.AnyAsync() == false) {
            blockList = new GuildBlockListModule{
                guildID = guildDB.guildID,
                blockTag = block
            };
            context.Add(blockList);
            await context.SaveChangesAsync();
        } else {
            return;
        }
    }


    public List<GuildBlockListModule> GetGuildBlockList(SocketGuild guild) {
        
        using var context = _contextFactory.CreateDbContext();
        return context.GuildBlockLists.Where(x => x.guildID == guild.Id).ToList();
    }

    public List<BlocklistModule> GetBlocklists(SocketUser user) {
        using var context = _contextFactory.CreateDbContext();
        return context.blocklists.Where(x => x.userID == user.Id).ToList();
    }
}
