using Discord.WebSocket;
using DragonData.Context;
using DragonData.Module;
using Microsoft.EntityFrameworkCore;

namespace DragonData;

public class DataBlocklist
{
    private readonly IDbContextFactory<DatabaseContext>_contextFactory;
    
    public DataBlocklist(IDbContextFactory<DatabaseContext> dbContext)
    {
        _contextFactory = dbContext;
    }


     private async Task<GuildModule> GetAndCreateGuild(SocketGuild Guild, IQueryable<GuildModule> guildModules)
     {
        GuildModule itemGuild;
        using var context = _contextFactory.CreateDbContext();

        if (await guildModules.AnyAsync() == false) {
            itemGuild = new GuildModule{
                guildID = Guild.Id,
                guildName = Guild.Name
            };
            context.Add(itemGuild);
            await context.SaveChangesAsync();
        } else {
            itemGuild = await guildModules.FirstAsync();
        }
            
        return itemGuild;
     }


    public async Task<UserModule> GetAndCreateUser(SocketUser user, IQueryable<UserModule> query) {
        UserModule itemUser;
        using var context = _contextFactory.CreateDbContext();

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


    public async Task GenerateSettingsDefaultForUser(SocketUser user, List<string> blocklistDefault)
    {
        using var context = _contextFactory.CreateDbContext();
        var query = context.Users.Where(x => x.userID == user.Id);
        
        if (await query.AnyAsync() == true) {
            return;
        }

        foreach (var itemTagsName in blocklistDefault)
        {
            await SetBlocklist(user, itemTagsName, query);
        }
    }

    public async Task GenerateSettingsDefaultForGuild(SocketGuild guild, List<string> bloclkistDefault) {
        using var context = _contextFactory.CreateDbContext();
        var query = context.Guilds.Where(x => x.guildID == guild.Id );
        foreach (var itemTagName in bloclkistDefault) {
            await SetBlocklistForGuild(guild, itemTagName, query);
        }

    }

    public async Task AddBlocklist(SocketUser user, string tag) {
        using var context = _contextFactory.CreateDbContext();
        var query = context.Users.Where(x => x.userID == user.Id);
        await SetBlocklist(user, tag, query);
    }

    public async Task AddBlocklistForGuild(SocketGuild guild, string tag) {
        using var context = _contextFactory.CreateDbContext();
        var query = context.Guilds.Where(x => x.guildID == guild.Id);
        await SetBlocklistForGuild(guild, tag, query);
    }

    private async Task SetBlocklist(SocketUser user, string blockTag, IQueryable<UserModule> queryUser) {
        using var context = _contextFactory.CreateDbContext();
        BlocklistModule blocklist;

        UserModule userDB = await GetAndCreateUser(user, queryUser);
        var query = context.blocklists.Where(x => x.blockTag == blockTag).Where(x => x.userID == user.Id);

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

    public async Task DeleteTagFromBlocklist(SocketUser user, string blocktag)
    {
        using var context = _contextFactory.CreateDbContext();
        var query = context.blocklists.Where(x => x.userID == user.Id).Where(x => x.blockTag == blocktag).FirstOrDefault();
        if (query != null)
        {
            context.Remove(query);
            await context.SaveChangesAsync();
        }
        return;
    }

    private async Task SetBlocklistForGuild(SocketGuild guild, string block, IQueryable<GuildModule> queryGuild) {
        using var context = _contextFactory.CreateDbContext();
        GuildBlockListModule blockList;

        GuildModule guildDB = await GetAndCreateGuild(guild, queryGuild);
        var query = context.GuildBlockLists.Where(x => x.blockTag == block).Where(x => x.guildID == guild.Id);
        
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
