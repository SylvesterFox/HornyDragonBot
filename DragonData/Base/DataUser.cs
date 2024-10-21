using Discord.WebSocket;
using DragonData.Context;
using DragonData.Module;
using Microsoft.EntityFrameworkCore;

namespace DragonData.Base
{
    public class DataUser : DataBase
    {
        public DataUser(IDbContextFactory<DatabaseContext> context) : base(context)
        {
        }


        public async Task<bool> DataCheckUser(ulong ID)
        {
            using var context = contextFactoryData.CreateDbContext();
            var query = context.Users.Where(x => x.userID == ID);

            return await query.AnyAsync();
        }

        private async Task<UserModule> CreateUser(SocketUser user)
        {
            using var context = contextFactoryData.CreateDbContext();
            UserModule itemUser;

            itemUser = new UserModule
            {
                userID = user.Id,
                username = user.Username
            };
            context.Add(itemUser);
            await context.SaveChangesAsync();
            return itemUser;
        }

        private async Task<UserModule> GetUser(SocketUser user)
        {
            using var context = contextFactoryData.CreateDbContext();
            var dataUserQuery = context.Users.Where(x => x.userID == user.Id);
            UserModule itemUser = await dataUserQuery.FirstAsync();
            return itemUser;
        }

        public async Task<UserModule> GetAndCreateDataUser(SocketUser user)
        {
            UserModule dataUser;
            var _blocklist = new DataBlocklist(contextFactoryData);

            if (await DataCheckUser(user.Id) != false)
            {
                return await GetUser(user);
            }

            dataUser = await CreateUser(user);
            foreach (var itemTagsName in DataBlocklist._globalBlockList)
            {
                await _blocklist.SetBlocklist(user, itemTagsName, dataUser);
            }
            return dataUser;
        }
    }
}
