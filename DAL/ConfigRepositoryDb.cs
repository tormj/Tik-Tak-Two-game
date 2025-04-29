using Domain;
using GameBrain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class ConfigRepositoryDb : IConfigRepository
    {
        private readonly AppDbContext _context;

        public ConfigRepositoryDb(AppDbContext context)
        {
            _context = context;
            SeedDefaultConfigurations(); // Seed defaults if none exist
        }

        public List<string> GetConfigurationNames() =>
            _context.Configurations.Select(c => c.Name).ToList();

        public GameConfiguration? GetConfigurationByName(string name)
        {
            var config = _context.Configurations
                .Include(c => c.SaveGames) 
                .FirstOrDefault(c => c.Name == name);

            return config != null ? GameConfiguration.FromDbConfig(config) : null;
        }

        public void SaveConfiguration(GameConfiguration gameConfig)
        {
            var config = gameConfig.ToDbConfig();
            var existingConfig = _context.Configurations.FirstOrDefault(c => c.Name == config.Name);

            if (existingConfig != null)
            {
                _context.Entry(existingConfig).CurrentValues.SetValues(config);
            }
            else
            {
                _context.Configurations.Add(config);
            }
            
            _context.SaveChanges();
        }

        private void SeedDefaultConfigurations()
        {
            if (_context.Configurations.Any()) return; // Skip if any configurations already exist

            var defaultConfigs = new List<Configuration>
            {
                new Configuration
                {
                    Name = "Classical",
                    BoardWidth = 3,
                    BoardHeight = 3,
                    GridSize = 3,
                    WinCondition = 3
                },
                new Configuration
                {
                    Name = "Big board",
                    BoardWidth = 10,
                    BoardHeight = 10,
                    GridSize = 5,
                    WinCondition = 4,
                    MovePieceAfterNMoves = 3
                }
            };

            _context.Configurations.AddRange(defaultConfigs);
            _context.SaveChanges();
        }
    }
}
