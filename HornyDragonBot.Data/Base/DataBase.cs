
using HornyDragonBot.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace HornyDragonBot.Data.Base
{

    public abstract class DataBase
    {
        public readonly IDbContextFactory<DatabaseContext> contextFactoryData;

        public DataBase(IDbContextFactory<DatabaseContext> context)
        {
            contextFactoryData = context;
        }

    }
}
