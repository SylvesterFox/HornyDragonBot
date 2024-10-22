
using DragonData.Context;
using Microsoft.EntityFrameworkCore;

namespace DragonData.Base
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
