using GameBrain;
using System.Collections.Generic;
using Domain;

namespace DAL;

public interface IConfigRepository
{
    List<string> GetConfigurationNames();
    GameConfiguration? GetConfigurationByName(string name);
    void SaveConfiguration(GameConfiguration config);
}