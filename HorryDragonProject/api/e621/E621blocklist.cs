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

   /*         UserModule userItem = await _dragonDataBase.dataUser.GetAndCreateDataUser(user);*/
            if (ignore != true)
            {
                var list = await _dragonDataBase.blocklist.GetBlocklist(user);
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

            /*GuildModule guildItem = await _dragonDataBase.dataGuild.GetAndCreateDataGuild(guild);*/
            if (ignore != true)
            {
                var list = await _dragonDataBase.blocklist.GetGuildBlockList(guild);
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

        public async Task<bool> CheckTagBlocklistForGuild(SocketGuild guild, string tag)
        {
            var listTag = await _dragonDataBase.blocklist.GetGuildBlockList(guild);
            List<string> blocklist = new List<string>();
            foreach (var item in listTag)
            {
                blocklist.Add(item.blockTag);
            }

            string[] tagArray = tag.Split(' ');
            List<string> queryList = new List<string>(tagArray);

            return queryList.Any(e => blocklist.Contains(e));
        }

        public async Task<bool> CheckTagBlocklist(SocketUser user, string tag)
        {
            var listTag = await _dragonDataBase.blocklist.GetBlocklist(user);
            List<string> blocklist = new List<string>();
            foreach (var item in listTag)
            {
                blocklist.Add(item.blockTag);
            }
            
            string[] tagArray = tag.Split(' ');
            List<string> queryList = new List<string>(tagArray);

            return queryList.Any(e => blocklist.Contains(e));
        }


        public async Task<string> GetStringBlocklistForGuild(SocketGuild guild)
        {
            var list = await _dragonDataBase.blocklist.GetGuildBlockList(guild);
            string text = "";
            text += string.Join(" ", list.Select(x => "-" + x.blockTag));
            return text;
        }

        public async Task<string> GetStringBlocklistForUser(SocketUser user)
        {
            var list = await _dragonDataBase.blocklist.GetBlocklist(user);
            string text = "";
            text += string.Join(" ", list.Select(x => "-" + x.blockTag));
            return text;
        }


    }
}
