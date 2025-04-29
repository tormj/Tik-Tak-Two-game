using Domain;
using GameBrain;

namespace DAL
{
    public interface IGameRepository
    {
        int SaveGame(GameState gameState, int saveGameId);
        GameState? LoadGame(int saveGameId);
        List<SaveGame> GetAllSaveGames();
        void DeleteSaveGame(int id);
    }
}