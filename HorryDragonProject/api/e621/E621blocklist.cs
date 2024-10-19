using Discord.WebSocket;
using DragonData;
using DragonData.Module;

namespace HorryDragonProject.api.e621
{
    public class E621blocklist
    {
        private DragonDataBase _dragonDataBase {  get; set; }
        private Dictionary<ulong, string> _blockTagUser = new Dictionary<ulong, string>();

        public E621blocklist(IServiceProvider service, DragonDataBase dragonData)
        {
            _dragonDataBase = dragonData;
        }

        public async Task<Dictionary<ulong, string>?> UseBlocklistUser(SocketUser? user, bool ignore = true)
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

        /* public async Task UseBlocklistForGuild(SocketGuild guild, bool ignore = true) {
             await _dragonDataBase.blocklist.GenerateSettingsDefaultForGuild(guild, _globalBlockList);
         }*/
    }
}
