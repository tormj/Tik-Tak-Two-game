using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Domain;
using GameBrain;

namespace DAL
{
    public class ConfigRepositoryJson : IConfigRepository
    {
        private const string ConfigFilePath = "Configurations.json";
        private List<Configuration> _configurations = new List<Configuration>();

        public ConfigRepositoryJson()
        {
            LoadConfigurations();

            // Add default configurations if the list is empty
            if (!_configurations.Any())
            {
                AddDefaultConfigurations();
                SaveConfigurations();
            }
        }

        public List<string> GetConfigurationNames() =>
            _configurations.Select(config => config.Name).ToList();

        public GameConfiguration? GetConfigurationByName(string name)
        {
            var config = _configurations.FirstOrDefault(c => c.Name == name);
            return config != null ? GameConfiguration.FromDbConfig(config) : null;
        }

        public void SaveConfiguration(GameConfiguration gameConfig)
        {
            var config = gameConfig.ToDbConfig();
            var existingConfig = _configurations.FirstOrDefault(c => c.Name == config.Name);

            if (existingConfig != null)
            {
                _configurations.Remove(existingConfig);
            }
            _configurations.Add(config);
            SaveConfigurations();
        }

        private void LoadConfigurations()
        {
            if (File.Exists(ConfigFilePath))
            {
                var json = File.ReadAllText(ConfigFilePath);
                _configurations = JsonSerializer.Deserialize<List<Configuration>>(json) ?? new List<Configuration>();
            }
        }

        private void SaveConfigurations()
        {
            var json = JsonSerializer.Serialize(_configurations);
            File.WriteAllText(ConfigFilePath, json);
        }

        private void AddDefaultConfigurations()
        {
            _configurations.Add(new Configuration
            {
                Name = "Classical",
                BoardWidth = 3,
                BoardHeight = 3,
                GridSize = 3,
                WinCondition = 3
            });

            _configurations.Add(new Configuration
            {
                Name = "Big board",
                BoardWidth = 10,
                BoardHeight = 10,
                GridSize = 5,
                WinCondition = 4,
                MovePieceAfterNMoves = 3
            });
        }
    }
}
