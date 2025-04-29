using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain;
using GameBrain;

namespace DAL
{
    public class GameRepositoryJson : IGameRepository
    {
        private const string FilePath = "SaveGames.json";

        public int SaveGame(GameState gameState, int saveGameId)
        {
            var saveGames = GetAllSaveGames();

            var existingGame = saveGames.FirstOrDefault(s => s.Id == saveGameId);
            if (existingGame != null)
            {
                existingGame.GameBoard = JsonSerializer.Serialize(gameState.GameBoard);
                existingGame.GridPositionX = gameState.GridPositionX;
                existingGame.GridPositionY = gameState.GridPositionY;
                existingGame.TotalMovesMade++;
                existingGame.NextMoveBy = (int)gameState.NextMoveBy; // Store as integer
                existingGame.GameMode = gameState.GameMode;
                existingGame.Configuration = gameState.GameConfiguration.ToDbConfig();
                
            }
            else
            {
                var newGame = new SaveGame
                {
                    Id = saveGames.Any() ? saveGames.Max(s => s.Id) + 1 : 1,
                    GameBoard = JsonSerializer.Serialize(gameState.GameBoard),
                    GridPositionX = gameState.GridPositionX,
                    GridPositionY = gameState.GridPositionY,
                    TotalMovesMade = 0,
                    NextMoveBy = (int)gameState.NextMoveBy,
                    GameMode = gameState.GameMode,
                    Configuration = gameState.GameConfiguration.ToDbConfig()
                };

                saveGames.Add(newGame);
                saveGameId = newGame.Id;
            }

            SaveAllGames(saveGames);
            return saveGameId;
        }


        public GameState? LoadGame(int saveGameId)
        {
            var saveGame = GetAllSaveGames().FirstOrDefault(s => s.Id == saveGameId);
            if (saveGame == null) return null;

            return new GameState
            {
                GameBoard = JsonSerializer.Deserialize<EGamePiece[][]>(saveGame.GameBoard ?? "[]", new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() }
                }),
                GridPositionX = saveGame.GridPositionX,
                GridPositionY = saveGame.GridPositionY,
                TotalMovesMade = saveGame.TotalMovesMade,
                NextMoveBy = (EGamePiece)saveGame.NextMoveBy,
                GameMode = saveGame.GameMode ?? "TwoPlayer",
                GameConfiguration = saveGame.Configuration != null
                    ? GameConfiguration.FromDbConfig(saveGame.Configuration)
                    : new GameConfiguration()
            };
        }

        public List<SaveGame> GetAllSaveGames()
        {
            if (!File.Exists(FilePath)) return new List<SaveGame>();
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<SaveGame>>(json) ?? new List<SaveGame>();
        }

        public void DeleteSaveGame(int id)
        {
            throw new NotImplementedException();
        }

        private void SaveAllGames(List<SaveGame> saveGames)
        {
            var json = JsonSerializer.Serialize(saveGames, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
