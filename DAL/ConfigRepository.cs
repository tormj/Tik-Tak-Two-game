using GameBrain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class ConfigRepository
    {
        
        private List<GameConfiguration> _gameConfigurations = new List<GameConfiguration>()
        {
            new GameConfiguration()
            {
                Name = "Classical"
            },
            new GameConfiguration()
            {
                Name = "Big board",
                BoardWidth = 10,
                BoardHeight = 10,
                WinCondition = 4,
                MovePieceAfterNMoves = 3,
            },
        };

        // Get a list of configuration names
        public List<string> GetConfigurationNames()
        {
            return _gameConfigurations
                .OrderBy(x => x.Name)
                .Select(config => config.Name)
                .ToList();
        }

        // Get a configuration by its name
        public GameConfiguration GetConfigurationByName(string name)
        {
            return _gameConfigurations.Single(c => c.Name == name);
        }

        // Create a new game configuration
        public GameConfiguration CreateNewConfiguration()
        {
            Console.WriteLine("Enter configuration name:");
            var name = Console.ReadLine()!;

            Console.WriteLine("Enter board width:");
            var boardWidth = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter board height:");
            var boardHeight = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter grid size:");
            var gridSize = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter win condition (number of pieces in a row to win):");
            var winCondition = int.Parse(Console.ReadLine()!);

            // Validate that win condition is not larger than the grid size
            if (winCondition > gridSize)
            {
                throw new ApplicationException("Win condition cannot be larger than the grid size.");
            }

            var newConfig = new GameConfiguration
            {
                Name = name,
                BoardWidth = boardWidth,
                BoardHeight = boardHeight,
                GridSize = gridSize,
                WinCondition = winCondition
            };
            
            _gameConfigurations.Add(newConfig);

            return newConfig;
        }
        
        public void SaveConfiguration(GameConfiguration config)
        {
            _gameConfigurations.Add(config);
        }
    }
}
