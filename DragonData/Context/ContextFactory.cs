using DragonData.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DragonData.Context;

public class ContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var path = SettingsDefault.LocalPathDB();

        var optionsBuild = new DbContextOptionsBuilder()
            .UseSqlite($"Data Source={path}");

        return new DatabaseContext(optionsBuild.Options);
    }
}
