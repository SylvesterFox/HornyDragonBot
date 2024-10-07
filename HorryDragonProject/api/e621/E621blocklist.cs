using Discord.WebSocket;
using DragonData;
using Microsoft.Extensions.DependencyInjection;

namespace HorryDragonProject.api.e621
{
    public class E621blocklist
    {
        private E621api _api;
        private DragonDataBase _dragonDataBase;

        private List<string> _globalBlockList = new List<string>() { "gore", "scat", "watersports", "loli", "shota", "my_little_pony", "young", "fart" };

        public E621blocklist(IServiceProvider service)
        {
            _api = service.GetRequiredService<E621api>();
            _dragonDataBase = service.GetRequiredService<DragonDataBase>();
        }

        public async Task UseBlocklist(SocketUser user, bool ignore = true)
        {
            await _dragonDataBase.blocklist.GenerateSettingsDefaultForUser(user, _globalBlockList);;
            if (ignore == true)
            {
                var list = _dragonDataBase.blocklist.GetBlocklists(user);
                string urlblocklist = string.Join(" ", list.Select(x => "-" + x.blockTag));

                if (_api.BlockTag.ContainsKey(user.Id) == false) {
                    _api.BlockTag.Add(user.Id, urlblocklist);
                } else {
                    _api.BlockTag[user.Id] = urlblocklist;
                }
            }
        }

        public async Task UseBlocklistForGuild(SocketGuild guild, bool ignore = true) {
            await _dragonDataBase.blocklist.GenerateSettingsDefaultForGuild(guild, _globalBlockList);
        }
    }
}
