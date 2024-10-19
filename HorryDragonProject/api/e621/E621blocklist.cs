using Discord.WebSocket;
using DragonData;
using DragonData.Module;

namespace HorryDragonProject.api.e621
{
    public class E621blocklist
    {
        private DragonDataBase _dragonDataBase {  get; set; }
        private Dictionary<ulong, string> _blockTagUser = new Dictionary<ulong, string>();
        private Dictionary<ulong, string> _blockTagGuild = new Dictionary<ulong, string>();

        public E621blocklist(IServiceProvider service, DragonDataBase dragonData)
        {
            _dragonDataBase = dragonData;
        }

        public async Task<Dictionary<ulong, string>?> UseBlocklistUser(SocketUser? user, bool ignore = false)
        {
            if (user == null)
            {
                return null;
            }

            UserModule userItem = await _dragonDataBase.dataUser.GetAndCreateDataUser(user);
            if (ignore != true)
            {
                var list = _dragonDataBase.blocklist.GetBlocklists(userItem.userID);
                string urlblocklist = string.Join(" ", list.Select(x => "-" + x.blockTag));

                if (_blockTagUser.ContainsKey(user.Id) == false)
                {
                    _blockTagUser.Add(user.Id, urlblocklist);
                }
                else
                {
                    _blockTagUser[user.Id] = urlblocklist;
                }

                return _blockTagUser;
            }

            return null;
        }

        public async Task<Dictionary<ulong, string>?> UseBlocklistGuild(SocketGuild? guild, bool ignore = false)
        {
            if (guild == null)
            {
                return null;
            }

            GuildModule guildItem = await _dragonDataBase.dataGuild.GetAndCreateDataGuild(guild);
            if (ignore != true)
            {
                var list = _dragonDataBase.blocklist.GetGuildBlockList(guildItem.guildID);
                string urlblocklist = string.Join(" ", list.Select(_ => "-" + _.blockTag));

                if (_blockTagGuild.ContainsKey(guild.Id) == false)
                {
                    _blockTagGuild.Add(guild.Id, urlblocklist);
                }
                else
                {
                    _blockTagGuild[guild.Id] = urlblocklist;
                }

                return _blockTagGuild;
            }

           return null;
        }

        /* public async Task UseBlocklistForGuild(SocketGuild guild, bool ignore = true) {
             await _dragonDataBase.blocklist.GenerateSettingsDefaultForGuild(guild, _globalBlockList);
         }*/
    }
}
