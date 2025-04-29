using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var ConnectionString = $"Data Source={FileHelper.BasePath}app.db";
        var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(ConnectionString)
            .EnableDetailedErrors() 
            .EnableSensitiveDataLogging()
            .Options;
        
        return new AppDbContext(contextOptions);
    }
}