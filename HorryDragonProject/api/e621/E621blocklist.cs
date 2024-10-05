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

        public void UseBlocklist(SocketUser user, bool ignore = true)
        {
            if (ignore == true)
            {
                _api.BlockTag.Clear();
                var list = _dragonDataBase.GetBlocklists(user);

                foreach (var item in list)
                {
                    _api.BlockTag.Add(item.blockTag);
                }
            }
        }

        public async Task BlocklistForUser(SocketUser user)
        {
            await _dragonDataBase.GenerateBlocklistForUser(user, _globalBlockList);
        }
    }
}
