
using Discord.WebSocket;
using DragonData.Context;
using DragonData.Module;
using DragonData.Settings;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DragonData.Base;

public class DataBlocklist : DataBase
{
    public static List<string> _globalBlockList = SettingsBlocklistDefault.GetJsonBlocklist();

    public DataBlocklist(IDbContextFactory<DatabaseContext> context) : base(context)
    {

    }


    public async Task AddBlocklist(SocketUser user, string tag)
    {
        var _user = new DataUser(contextFactoryData);
        using var context = contextFactoryData.CreateDbContext();
        UserModule itemUser = await _user.GetAndCreateDataUser(user);
        await SetBlocklist(user, tag, itemUser);
    }

    public async Task AddBlocklistForGuild(SocketGuild guild, string tag)
    {
        var _guild = new DataGuild(contextFactoryData);
        using var context = contextFactoryData.CreateDbContext();
        GuildModule itemGuild = await _guild.GetAndCreateDataGuild(guild);
        await SetBlocklistForGuild(guild, itemGuild, tag);
    }

    public async Task SetBlocklist(SocketUser user, string blockTag, UserModule userData)
    {
        using var context = contextFactoryData.CreateDbContext();
        BlocklistModule blocklist;


        var query = context.blocklists.Where(x => x.blockTag == blockTag).Where(x => x.userID == user.Id);

        if (await query.AnyAsync() == false)
        {
            blocklist = new BlocklistModule
            {
                userID = userData.userID,
                blockTag = blockTag
            };
            context.Add(blocklist);
        }
        else
        {
            return;
        }

        await context.SaveChangesAsync();
    }

    public async Task DeleteTagFromBlocklist(SocketUser user, string blocktag)
    {
        using var context = contextFactoryData.CreateDbContext();
        var query = await context.blocklists.Where(x => x.userID == user.Id).Where(x => x.blockTag == blocktag).FirstOrDefaultAsync();
        if (query != null)
        {
            context.Remove(query);
            await context.SaveChangesAsync();
        }
        return;
    }

    public async Task GuildDeleteTagFromBlocklist(SocketGuild guild, string blocktag)
    {
        using var context = contextFactoryData.CreateDbContext();
        var query = await context.GuildBlockLists.Where(x => x.guildID == guild.Id).Where(x => x.blockTag == blocktag).FirstOrDefaultAsync();
        if (query != null)
        {
            context.Remove(query);
            await context.SaveChangesAsync();
        }
        return;
    }


    public async Task SetBlocklistForGuild(SocketGuild guild, GuildModule guildModule, string block)
    {
        using var context = contextFactoryData.CreateDbContext();
        GuildBlockListModule blockList;


        var query = context.GuildBlockLists.Where(x => x.blockTag == block).Where(x => x.guildID == guild.Id);

        if (await query.AnyAsync() == false)
        {
            blockList = new GuildBlockListModule
            {
                guildID = guildModule.guildID,
                blockTag = block
            };
            context.Add(blockList);
            await context.SaveChangesAsync();
        }

        return;
    }


    public async Task<List<GuildBlockListModule>> GetGuildBlockList(SocketGuild guild)
    {
        var _guild = new DataGuild(contextFactoryData);
        GuildModule dataGuild = await _guild.GetAndCreateDataGuild(guild);
        using var context = contextFactoryData.CreateDbContext();
        return context.GuildBlockLists.Where(x => x.guildID == dataGuild.guildID).ToList();
    }

    public async Task<List<BlocklistModule>> GetBlocklist(SocketUser user)
    {
        var _user = new DataUser(contextFactoryData);
        UserModule dataUser = await _user.GetAndCreateDataUser(user);
        using var context = contextFactoryData.CreateDbContext();
        return context.blocklists.Where(x => x.userID == dataUser.userID).ToList();
    }




}
