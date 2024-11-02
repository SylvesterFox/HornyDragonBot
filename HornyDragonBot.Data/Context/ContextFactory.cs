using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HornyDragonBot.Data.Context;

public class ContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var path = DbSettings.LocalPathDB();

        var optionsBuild = new DbContextOptionsBuilder()
            .UseSqlite($"Data Source={path}");

        return new DatabaseContext(optionsBuild.Options);
    }
}
