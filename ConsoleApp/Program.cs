using DAL;
using Microsoft.EntityFrameworkCore;
using System;

namespace ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        { 
            var connectionString = $"Data Source={FileHelper.BasePath}app.db";
            
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connectionString)
                .EnableDetailedErrors() 
                .EnableSensitiveDataLogging()
                .Options;

            using var ctx = new AppDbContext(contextOptions);

            // Use JSON-based repositories
            //var configRepository = new ConfigRepositoryJson();
            //var gameRepository = new GameRepositoryJson();

            // OR

            // Use database-based repositories
            var configRepository = new ConfigRepositoryDb(ctx);
            var gameRepository = new GameRepositoryDb(ctx);
            
            GameController.Initialize(configRepository, gameRepository);
            
            while (true)
            {
                var result = Menus.MainMenu.Run();
                if (result == "Exit")  
                { 
                    break; // Exit the game
                }
                else if (result == "Return")
                {
                    Menus.MainMenu.Run();
                }
            }
        }
    }
}