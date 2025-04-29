using System.Text.Json;
using DAL;
using Domain;
using GameBrain;
using Microsoft.EntityFrameworkCore;

public class GameRepositoryDb : IGameRepository
{
    private readonly AppDbContext _context;

    public GameRepositoryDb(AppDbContext context)
    {
        _context = context;
    }

    public int SaveGame(GameState gameState, int saveGameId)
{
    try
    {
        SaveGame saveGame;
        
        if (saveGameId > 0)
        {
            // Load the existing game
            saveGame = _context.SaveGames.FirstOrDefault(s => s.Id == saveGameId);
            if (saveGame == null)
            {
                Console.WriteLine("what is happening");
                return -1;  // Game not found
            }

            // Increment the total moves made before making changes
            saveGame.TotalMovesMade++;

            // Update other properties
            saveGame.GameBoard = JsonSerializer.Serialize(gameState.GameBoard);
            saveGame.GridPositionX = gameState.GridPosition.X;
            saveGame.GridPositionY = gameState.GridPosition.Y;
            saveGame.NextMoveBy = (int)gameState.NextMoveBy;
            saveGame.CreatedAtDateTime = DateTime.Now.ToString("O");
            saveGame.GameMode = gameState.GameMode; 

            Console.WriteLine($"[DEBUG] Saving GameMode: {gameState.GameMode}");

            _context.Entry(saveGame).State = EntityState.Modified;
        }
        else
        {
            // Create a new save game
            saveGame = new SaveGame
            {
                TotalMovesMade = gameState.TotalMovesMade,
                GameBoard = JsonSerializer.Serialize(gameState.GameBoard),
                GridPositionX = gameState.GridPosition.X,
                GridPositionY = gameState.GridPosition.Y,
                NextMoveBy = (int)gameState.NextMoveBy,
                CreatedAtDateTime = DateTime.Now.ToString("O"),
                GameMode = gameState.GameMode
            };
            Console.WriteLine($"[DEBUG] Saving GameMode: {gameState.GameMode}");

            _context.SaveGames.Add(saveGame);
        }

        // Load or create the configuration
        var configuration = _context.Configurations
            .FirstOrDefault(c => c.Name == gameState.GameConfiguration.Name);

        if (configuration == null)
        {
            configuration = new Configuration
            {
                Name = gameState.GameConfiguration.Name,
                BoardWidth = gameState.GameConfiguration.BoardWidth,
                BoardHeight = gameState.GameConfiguration.BoardHeight,
                WinCondition = gameState.GameConfiguration.WinCondition
            };
            
            _context.Configurations.Add(configuration);
            _context.SaveChanges();
        }

        saveGame.ConfigurationId = configuration.Id;

        // Save changes to the database
        _context.SaveChanges();

        Console.WriteLine($"After Save - TotalMovesMade: {saveGame.TotalMovesMade}");
        return saveGame.Id;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error saving game: {ex.Message}");
        return -1;  // Return failure
    }
}


    public GameState? LoadGame(int saveGameId)
    {
        var saveGame = GetAllSaveGames().FirstOrDefault(s => s.Id == saveGameId);
        if (saveGame == null) return null;

        // Load configuration using the ConfigRepositoryJson
        var configRepository = new ConfigRepositoryJson();
        var configuration = configRepository.GetConfigurationByName(saveGame.Configuration?.Name ?? string.Empty);

        if (configuration == null)
        {
            Console.WriteLine("Configuration not found for the saved game.");
            return null;
        }

        return new GameState
        {
            GameBoard = JsonSerializer.Deserialize<EGamePiece[][]>(saveGame.GameBoard),
            GameConfiguration = configuration.Value,
            GridPositionX = saveGame.GridPositionX,
            GridPositionY = saveGame.GridPositionY,
            TotalMovesMade = saveGame.TotalMovesMade,
            NextMoveBy = (EGamePiece)saveGame.NextMoveBy,
            GameMode = saveGame.GameMode
        };
    }


    public void DeleteSaveGame(int id)
    {
        var saveGame = _context.SaveGames.Find(id);  // Find the game by its ID
        if (saveGame != null)
        {
            _context.SaveGames.Remove(saveGame);  // Remove the game
            _context.SaveChanges();  // Save the changes to the database
        }
        else
        {
            throw new Exception("Game not found.");
        }
    }

    public List<SaveGame> GetAllSaveGames()
    {
        return _context.SaveGames
            .Include(s => s.Configuration)  // Including related Configuration data (optional, based on your model)
            .ToList();  // Return the list of all saved games
    }
}
